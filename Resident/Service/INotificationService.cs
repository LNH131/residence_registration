namespace Resident.Service
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int userId, string message);
        Task SendEmailNotificationAsync(string recipientEmail, string subject, string body);
    }
}
