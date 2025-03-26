// Project/App.xaml.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public App() { }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configure EF Core with the connection string.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<PrnContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            // Register Services (use singleton for user state, transient for others)
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IPoliceProcessingService, PoliceProcessingService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IHouseholdService, HouseholdService>();
            services.AddTransient<UserDAO>();
            services.AddTransient<ChatMessageService>();

            // Register ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<CitizenViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<HouseHoldControlViewModel>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<HouseHoldSelectionViewModel>();
            services.AddTransient<UpdateCitizenProfileViewModel>();
            services.AddTransient<StatusOverviewViewModel>();
            services.AddTransient<PoliceViewModel>();
            services.AddTransient<PoliceChatSelectionViewModel>();
            services.AddTransient<PoliceChatViewModel>();
            services.AddTransient<PoliceApprovalsOverviewViewModel>();
            services.AddTransient<AreaLeaderViewModel>();
            services.AddTransient<AreaLeaderApprovalsOverviewViewModel>();
            services.AddTransient<RegistrationDetailsViewModel>();
            services.AddTransient<HouseholdTransferDetailsViewModel>();
            services.AddTransient<HouseholdSeparationDetailsViewModel>();
            services.AddTransient<CreateNotificationViewModel>();
            services.AddTransient<AreaLeaderChatSelectionViewModel>();
            services.AddTransient<AreaLeaderChatViewModel>();
            services.AddTransient<CitizenPoliceChatSelectionViewModel>();

            // Register Views / Windows
            services.AddTransient<LoginWindow>();
            services.AddTransient<AdminWindow>();
            services.AddTransient<PoliceWindow>();
            services.AddTransient<AreaLeaderWindow>();
            services.AddTransient<CitizenWindow>();
            services.AddTransient<Register>(); // Assuming this is a Window.
            services.AddTransient<HouseHoldControlWindow>();
            services.AddTransient<AddUserWindow>();
            services.AddTransient<ChangeUserWindow>();
            services.AddTransient<DeletedUserWindow>();
            services.AddTransient<HouseHoldSelectionWindow>();
            services.AddTransient<StatusOverviewWindow>();
            services.AddTransient<PoliceChatSelectionWindow>();
            services.AddTransient<PoliceApprovalsOverviewWindow>();
            services.AddTransient<AreaLeaderApprovalsOverviewWindow>();
            services.AddTransient<RegistrationDetailsWindow>(); // Must have a constructor accepting RegistrationDetailsViewModel.
            services.AddTransient<HouseholdTransferDetailsWindow>(); // Must have a constructor accepting HouseholdTransferDetailsViewModel.
            services.AddTransient<HouseholdSeparationDetailsWindow>(); // Must have a constructor accepting HouseholdSeparationDetailsViewModel.
            services.AddTransient<HouseholdDetailsWindow>(); // For household monitoring.
            services.AddTransient<CreateNotificationWindow>();
            services.AddTransient<AreaLeaderChatWindow>();
            services.AddTransient<CitizenPoliceChatSelectionWindow>();
            services.AddTransient<CitizenChatWindow>();
            services.AddTransient<INotificationService, NotificationService>();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Run migration to ensure the database schema is up-to-date.
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PrnContext>();
                await dbContext.Database.MigrateAsync();
            }

            try
            {
                var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
                MainWindow = loginWindow; // Set MainWindow before showing it.
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup error: {ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
