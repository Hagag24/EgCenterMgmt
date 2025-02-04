

namespace EgCenterMgmt.Services
{

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // إعداد SMTP
            var smtpClient = new SmtpClient("smtp.example.com") // استبدل بعنوان SMTP الخاص بك
            {
                Port = 587, // رقم المنفذ
                Credentials = new NetworkCredential("your_email@example.com", "your_password"), // استبدل بمعلومات الدخول الخاصة بك
                EnableSsl = true,
            };

            // إعداد الرسالة
            var mailMessage = new MailMessage
            {
                From = new MailAddress("your_email@example.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            // إرسال البريد الإلكتروني
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}