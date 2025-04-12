using saga.Infrastructure.Providers.Interfaces;
using saga.Settings;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace saga.Infrastructure.Providers
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ISettings settings, ILogger<EmailSender> logger)
        {
            _emailSettings = settings.EmailSettings;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task SendEmail(string recipient, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                    var message = new MailMessage(_emailSettings.SenderEmail, recipient, subject, body);
                    message.IsBodyHtml = isBodyHtml;

                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                    {
                        var sendTask = client.SendMailAsync(message);
                        var completedTask = await Task.WhenAny(sendTask, Task.Delay(-1, cts.Token));
                        if (completedTask == sendTask)
                        {
                            await sendTask;
                        }
                        else
                        {
                            _logger.LogError($"Smtp server is not work, was not possible the email to: {recipient}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error sending email to: {recipient} error: {ex.Message}");
            }
        }
    }
}
