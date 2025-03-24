using Microsoft.Extensions.Configuration;
using Resident.Models;
using System.Net;
using System.Net.Mail;

namespace Resident.Service
{
    public class NotificationService : INotificationService
    {
        private readonly PrnContext _context;
        private readonly IConfiguration _configuration;

        public NotificationService(PrnContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SendInAppNotificationAsync(int userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                SentDate = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendEmailNotificationAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                // Lấy cấu hình SMTP từ appsettings.json
                string host = _configuration["Smtp:Host"] ?? "smtp.fpt.edu.vn";
                int port = int.TryParse(_configuration["Smtp:Port"], out int parsedPort) ? parsedPort : 587;
                string username = _configuration["Smtp:Username"] ?? "giangvthe187264@fpt.edu.vn";
                string password = _configuration["Smtp:Password"] ?? "dgoalidwbptuooya";
                bool enableSsl = bool.TryParse(_configuration["Smtp:EnableSsl"], out bool parsedSsl) ? parsedSsl : true;
                string fromAddress = _configuration["Smtp:FromAddress"] ?? "giangvthe187264@fpt.edu.vn";

                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new NetworkCredential(username, password);
                    client.EnableSsl = enableSsl;

                    var mail = new MailMessage
                    {
                        From = new MailAddress(fromAddress),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mail.To.Add(recipientEmail);

                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                // Bạn có thể tích hợp logging ở đây nếu muốn.
                throw new Exception("Email sending failed: " + ex.Message, ex);
            }
        }
    }
}
