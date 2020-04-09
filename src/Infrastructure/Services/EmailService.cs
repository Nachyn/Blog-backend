using System;
using System.Threading.Tasks;
using Application.Common.AppSettingHelpers.Main;
using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceSettings _emailSettings;

        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailServiceSettings> emailSettings
            , ILogger<EmailService> logger)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(params EmailMessageDto[] emailMessages)
        {
            if (emailMessages.IsNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(emailMessages));
            }

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailSettings.SmtpServer
                    , _emailSettings.SmtpPort
                    , _emailSettings.IsUseSsl);

                await client.AuthenticateAsync(_emailSettings.Email
                    , _emailSettings.Password);

                foreach (var emailMessage in emailMessages)
                {
                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName
                        , _emailSettings.Email));

                    mimeMessage.To.Add(new MailboxAddress(string.Empty
                        , emailMessage.Email));

                    mimeMessage.Subject = emailMessage.Subject;
                    mimeMessage.Body = new TextPart(TextFormat.Html)
                        {Text = emailMessage.MessageHtml};

                    try
                    {
                        await client.SendAsync(mimeMessage);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception
                            , $"{nameof(EmailService)} {exception.Message}");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception
                    , $"{nameof(EmailService)} {exception.Message}");
            }

            await client.DisconnectAsync(true);
        }
    }
}