// Project/App.xaml.cs
using Microsoft.EntityFrameworkCore; // IMPORTANT: Add this for UseSqlServer
using Microsoft.Extensions.Configuration; // IMPORTANT: Add this for configuration
using Microsoft.Extensions.DependencyInjection;
using Resident.DAO;
using Resident.Models;
using Resident.Service;
using Resident.Services;
using Resident.View;
using Resident.ViewModels;
using System.IO;
using System.Windows;

namespace Resident
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
            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<PrnContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            // Register services
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<UserDAO>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<PoliceWindow>();
            services.AddTransient<AreaLeaderWindow>();
            services.AddTransient<CitizenWindow>();
            services.AddTransient<CitizenViewModel>();
            services.AddTransient<Register>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<HouseHoldControlWindow>();
            services.AddTransient<HouseHoldControlViewModel>();
            services.AddTransient<AddUserWindow>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<HouseHoldSelectionWindow>();
            services.AddTransient<HouseHoldSelectionViewModel>();
            services.AddTransient<UpdateCitizenProfileWindow>();
            services.AddTransient<StatusOverviewWindow>();
            services.AddTransient<StatusOverviewViewModel>();
            services.AddTransient<PoliceWindow>();
            services.AddTransient<PoliceViewModel>();
            services.AddTransient<PoliceChatSelectionWindow>();
            services.AddTransient<PoliceChatSelectionViewModel>();
            services.AddTransient<PoliceChatViewModel>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<PoliceViewModel>();
            services.AddTransient<PoliceChatSelectionViewModel>();
            services.AddTransient<PoliceChatSelectionWindow>();
            services.AddTransient<HouseholdTransferDetailsViewModel>();
            services.AddTransient<HouseholdTransferDetailsWindow>();
            services.AddTransient<HouseholdSeparationDetailsViewModel>();
            services.AddTransient<HouseholdSeparationDetailsWindow>();
            services.AddTransient<IPoliceProcessingService, PoliceProcessingService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<INotificationService, NotificationService>();

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