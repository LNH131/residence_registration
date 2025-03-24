using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Resident.Enums;
using Resident.Models;
using Resident.ViewModels;

namespace Resident.View
{
    public partial class ChangeUserWindow : Window
    {
        private PrnContext _context = new PrnContext();
        private List<User> users;
        private List<User> users1 = new List<User>();
        private List<User> usersNon = new List<User>();
        private string originalPassword = "";
        public ObservableCollection<string> Roles { get; set; }
        public ObservableCollection<string> AreaID { get; set; }
        public ObservableCollection<string> Address { get; set; }
        public ObservableCollection<string> Gender { get; set; }


        public ChangeUserWindow()
        {
            InitializeComponent();
            Roles = new ObservableCollection<string>
            {
                "Citizen", "Police", "Admin", "AreaLeader"
            };
            AreaID = new ObservableCollection<string>
            {
                "1", "2", "3"
            };
            Address = new ObservableCollection<string>
            {
                "1002"
            };
            Gender = new ObservableCollection<string>
            {
                "Male", "Female", "Other",""
            };
            DataContext = this;
            role.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData()
        {
            var users = _context.Users.ToList();
            dtUserImport.ItemsSource = users;
            dtUserNonimport.ItemsSource = _context.Users.ToList();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(fullName.Text) || string.IsNullOrEmpty(email.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string finalPassword = (changePass.IsChecked == true && !string.IsNullOrEmpty(password.Text))
                            ? password.Text
                            : originalPassword;

            User newUser = new User
            {
                FullName = fullName.Text,
                Email = email.Text,
                Password = finalPassword,
                Role = role.Text,
                CurrentAddressId = 1002
            };
            users1.Add(newUser);
            MessageBox.Show("Add user successfully");
            LoadUser();

        }
        public void LoadUser()
        {
            dgUser.ItemsSource = null;
            dgUser.ItemsSource = users1;
            dgUser2.ItemsSource = null;
            dgUser2.ItemsSource = usersNon;
        }

        private void dtUserImport_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dtUserImport.SelectedItem is User selectedUser)
            {
                fullName.Text = selectedUser.FullName;
                email.Text = selectedUser.Email;
                originalPassword = selectedUser.Password;
                password.Text = "";
                role.Text = selectedUser.Role;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Duyệt qua danh sách local users
                foreach (var user in users1)
                {
                    // Tìm record trong database có email trùng với user hiện tại
                    var dbUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                    if (dbUser != null)
                    {
                        dbUser.FullName = user.FullName;
                        dbUser.Email = user.Email;
                        dbUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        dbUser.Role = user.Role;
                        dbUser.CurrentAddressId = user.CurrentAddressId;
                    }
                }

                // Lưu các thay đổi vào database
                _context.SaveChanges();
                MessageBox.Show("Database updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadData();
            }
            catch (Exception ex)
            {
                // Lấy lỗi gốc (InnerException) nếu có
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\nInner Exception: " + ex.InnerException.Message;
                }

                MessageBox.Show("Error when saving changes: " + errorMessage,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var logoutWindow = serviceProvider.GetRequiredService<AdminWindow>();
            logoutWindow.Show();
            this.Close();

        }

        private void dtUserNonimport_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dtUserNonimport.SelectedItem is User selectedUser)
            {
                userID.Text = selectedUser.UserId.ToString();
                fullname1.Text = selectedUser.FullName;
                areaID.Text = selectedUser.AreaId.ToString();
                addressID.Text = selectedUser.CurrentAddressId.ToString();
                birthday.Text = selectedUser.Birthday.ToString();
                gender.SelectedItem = selectedUser.Sex ?? "";

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var logoutWindow = serviceProvider.GetRequiredService<AdminWindow>();
            logoutWindow.Show();
            this.Close();
        }

        private void Addchange_Click(object sender, RoutedEventArgs e)
        {

            User newUser = new User
            {
                UserId = int.Parse(userID.Text),
                FullName = fullname1.Text,
                AreaId = int.Parse(areaID.Text),
                CurrentAddressId = int.Parse(addressID.Text),
                Birthday = birthday.SelectedDate.HasValue
               ? DateOnly.FromDateTime(birthday.SelectedDate.Value)
               : (DateOnly?)null,
                Sex = gender.SelectedItem?.ToString() ?? ""


            };
            usersNon.Add(newUser);
            MessageBox.Show("Add user successfully");
            LoadUser();
        }

        private void Save_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Duyệt qua danh sách local users
                foreach (var user in usersNon)
                {
                    // Tìm record trong database có email trùng với user hiện tại
                    var dbUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
                    if (dbUser != null)
                    {
                        dbUser.FullName = user.FullName;
                        dbUser.AreaId = user.AreaId;
                        dbUser.CurrentAddressId = user.CurrentAddressId;
                        dbUser.Birthday = user.Birthday;
                        dbUser.Sex = user.Sex;
                    }
                }

                // Lưu các thay đổi vào database
                _context.SaveChanges();
                MessageBox.Show("Database updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadData();
            }
            catch (Exception ex)
            {
                // Lấy lỗi gốc (InnerException) nếu có
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\nInner Exception: " + ex.InnerException.Message;
                }

                MessageBox.Show("Error when saving changes: " + errorMessage,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
