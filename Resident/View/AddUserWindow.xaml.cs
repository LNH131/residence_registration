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
using System.Xml.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Resident.Enums;
using Resident.Models;
using Resident.ViewModels;

namespace Resident.View
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        PrnContext _context = new PrnContext();
        private readonly AddUserViewModel _viewModel;
        static List<User> users;
        public ObservableCollection<string> Roles { get; set; }
        public AddUserWindow(PrnContext context)
        {
            InitializeComponent();
            Roles = new ObservableCollection<string>
            {
                "Citizen", "Police", "Admin", "AreaLeader"
            };
            DataContext = this;
            role.SelectedIndex = 0;
            users = new List<User>();
            _context = context;
            this.Loaded += AddUserWindow_Loaded;
        }
        public void AddUserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dtUser.ItemsSource = _context.Users.ToList();
        }
        public void Add_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            if (String.IsNullOrEmpty(fullName.Text) || String.IsNullOrEmpty(email.Text) || String.IsNullOrEmpty(passworld.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (users.FirstOrDefault(u => u.Email == email.Text) != null)
            {
                MessageBox.Show("Email already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            user.FullName = fullName.Text;
            user.Email = email.Text;
            user.Password =passworld.Text;
            user.CurrentAddressId = 1002;
            if (role.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Enum.TryParse(role.SelectedItem.ToString(), out Role parsedRole))
            {
                MessageBox.Show("Invalid role selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            user.Role = role.Text;

            users.Add(user);
            MessageBox.Show("Add user successfully");
            LoadUser();
        }

        public void LoadUser()
        {
            dgUser.ItemsSource = null;
            dgUser.ItemsSource = users;
        }


        private void dgUser_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgUser.SelectedItem is User selectedUser)
            {
                string emai = selectedUser.Email;
                User user = users.FirstOrDefault(u => u.Email == emai);
                if (user != null)
                {
                    fullName.Text = user.FullName;
                    email.Text = user.Email;
                    passworld.Text = user.Password;
                    role.Text = user.Role.ToString();
                }
            }


        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // Tìm user cần cập nhật
            User user = users.FirstOrDefault(u => u.Email == email.Text);

            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(fullName.Text) || String.IsNullOrEmpty(email.Text) || String.IsNullOrEmpty(passworld.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (users.Any(u => u.Email == email.Text && u != user))
            {
                MessageBox.Show("Email already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            user.FullName = fullName.Text;
            user.Password = passworld.Text;
            user.CurrentAddressId = 1002;

            if (Enum.TryParse(typeof(Role), role.Text, out object parsedRole))
            {
                user.Role = role.Text;
            }
            else
            {
                MessageBox.Show("Invalid role selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Update user successfully");
            LoadUser();
        }

        private void Deleted_Click(object sender, RoutedEventArgs e)
        {
            User user = users.FirstOrDefault(u => u.Email == email.Text);
            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            users.Remove(user);
            LoadUser();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            List<string> alreadyExistEmails = new List<string>();
            List<string> addedEmails = new List<string>();

            try
            {
                // Duyệt qua danh sách user cần lưu
                foreach (var tempUser in users)
                {
                    // Kiểm tra xem user đã tồn tại trong DB hay chưa dựa theo Email
                    var dbUser = _context.Users.FirstOrDefault(u => u.Email == tempUser.Email);
                    if (dbUser == null)
                    {
                        User user = new User()
                        {
                            FullName = tempUser.FullName,
                            Email = tempUser.Email,
                            Password = BCrypt.Net.BCrypt.HashPassword(tempUser.Password),
                            Role = tempUser.Role,
                            CurrentAddressId = tempUser.CurrentAddressId
                        };
                        _context.Users.Add(user);
                        addedEmails.Add(tempUser.Email);
                    }
                    else
                    {
                        // Nếu đã tồn tại, lưu email để thông báo sau
                        alreadyExistEmails.Add(tempUser.Email);
                    }
                }

                // Lưu các thay đổi xuống DB (EF sẽ tự chuyển đổi enum Role sang string nhờ HasConversion)
                _context.SaveChanges();

                // Xây dựng thông báo kết quả
                string message = "";
                if (addedEmails.Count > 0)
                    message += "Thêm thành công: " + string.Join(", ", addedEmails);
                if (alreadyExistEmails.Count > 0)
                {
                    if (!string.IsNullOrEmpty(message))
                        message += "\n";
                    message += "Đã tồn tại (bỏ qua): " + string.Join(", ", alreadyExistEmails);
                }
                MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi chi tiết, bao gồm cả InnerException (nếu có)
                MessageBox.Show("Error saving changes: " + ex.Message + "\nInner Exception: " + ex.InnerException?.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            dtUser.ItemsSource = _context.Users.ToList();
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