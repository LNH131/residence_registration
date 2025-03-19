﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Models;
using System.Windows;

namespace Project.View
{
    public partial class AdminWindow : Window
    {
        private readonly PrnContext _context;
        public AdminWindow(PrnContext context)
        {
            InitializeComponent();
            _context = context;
            this.btnLogout.Click += this.Logout_Click;
            this.Loaded += AdminWindow_Loaded;  // Đăng ký sự kiện Loaded

        }
        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu và gán cho ListView
            lvUsers.ItemsSource = GetAllUsers();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
            this.Close();
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var serviceProvider = ((App)Application.Current).ServiceProvider;
            var addUserWindow = serviceProvider.GetRequiredService<AddUserWindow>();
            addUserWindow.Show();
            this.Close();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}