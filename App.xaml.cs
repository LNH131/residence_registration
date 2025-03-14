﻿// Project/App.xaml.cs
using Microsoft.EntityFrameworkCore; // IMPORTANT: Add this for UseSqlServer
using Microsoft.Extensions.Configuration; // IMPORTANT: Add this for configuration
using Microsoft.Extensions.DependencyInjection;
using Project.DAO;
using Project.Models;
using Project.View;
using Project.ViewModels;
using System.IO;
using System.Windows;

namespace Project
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {

        }


        private void ConfigureServices(IServiceCollection services)
        {
            // --- Configure EF Core with the connection string ---
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddDbContext<PrnContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            // Register services
            services.AddTransient<UserDAO>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<PoliceWindow>();
            services.AddTransient<AreaLeaderWindow>();
            services.AddTransient<CitizenWindow>();
            services.AddTransient<Register>();
            services.AddTransient<RegisterViewModel>();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            //Run migration
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PrnContext>();
                dbContext.Database.Migrate();
            }
            try
            {
                var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
                MainWindow = loginWindow; // Set MainWindow *before* showing
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup error: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }


        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (ServiceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }
    }
}