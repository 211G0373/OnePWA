namespace OnePWA.Services
{
    public class IEmailService
    {
        public interface IEmailServices
        {
            Task SendEmailAsync(string toEmail, string subject, string body);
        }
    }
}
