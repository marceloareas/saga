using saga.Models.DTOs;
using saga.Models.Entities;
using System.Threading.Tasks;

namespace saga.Infrastructure.Providers.Interfaces
{
    /// <summary>
    /// Represents a service for sending emails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email using the provided access token.
        /// </summary>
        /// <param name="accessToken">The access token for authentication.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="recipientEmail">The email address of the recipient.</param>
        /// <returns>A task representing the asynchronous email sending operation.</returns>
        Task SendEmail(string recipient, string subject, string body, bool isBodyHtml = true);
    }
}
