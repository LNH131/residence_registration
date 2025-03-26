using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Resident.Models;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for DeletedUserWindow.xaml
    /// </summary>
    public partial class DeletedUserWindow : Window
    {
        private PrnContext _context = new PrnContext();
        private List<User> users;
        private List<User> users1 = new List<User>();
        public ObservableCollection<string> Roles { get; set; }
        public DeletedUserWindow()
        {
            InitializeComponent();
            Roles = new ObservableCollection<string>
            {
                "Citizen", "Police", "Admin", "AreaLeader"
            };
            DataContext = this;
            role.SelectedIndex = 0;
            LoadData();

        }
        public void LoadData()
        {
            users = _context.Users.ToList();
            dtUserImport.ItemsSource = users;
        }
        public void LoadUser()
        {
            dgUser.ItemsSource = null;
            dgUser.ItemsSource = users1;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(fullName.Text) || string.IsNullOrEmpty(email.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User newUser = new User
            {
                FullName = fullName.Text,
                Email = email.Text,
                Role = role.Text,
                CurrentAddressId = 1002
            };
            users1.Add(newUser);
            MessageBox.Show("Add user successfully");
            LoadUser();
        }
        private void dtUserImport_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dtUserImport.SelectedItem is User selectedUser)
            {
                fullName.Text = selectedUser.FullName;
                email.Text = selectedUser.Email;
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
                        _context.Users.Remove(dbUser);

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
    }
}
