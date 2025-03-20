namespace Resident.Service
{
    public interface INotificationService
    {
        Task SendInAppNotificationAsync(int userId, string message);
        Task SendEmailNotificationAsync(string recipientEmail, string subject, string body);
    }

}
