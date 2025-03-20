using Resident.Models;
using System.Net;
using System.Net.Mail;

namespace Resident.Service
{
    public class NotificationService : INotificationService
    {
        private readonly PrnContext _context;
        public NotificationService(PrnContext context)
        {
            _context = context;
        }

        public async Task SendInAppNotificationAsync(int userId, string message)
        {
            // Tạo đối tượng Notification mới
            var notification = new Notification
            {
                Message = message,
                SentDate = DateTime.Now,
                IsRead = false,
                UserId = userId
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendEmailNotificationAsync(string recipientEmail, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.fpt.edu.vn", 587)) // Sử dụng host và port theo cấu hình của FPT
            {
                client.Credentials = new NetworkCredential("giangvthe187264@fpt.edu.vn", "dgoalidwbptuooya");
                client.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress("giangvthe187264@fpt.edu.vn"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(recipientEmail);

                await client.SendMailAsync(mail);
            }
        }
    }
}
