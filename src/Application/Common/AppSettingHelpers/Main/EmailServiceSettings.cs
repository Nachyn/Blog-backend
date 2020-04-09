namespace Application.Common.AppSettingHelpers.Main
{
    public class EmailServiceSettings
    {
        public bool IsUseSsl { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpServer { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string SenderName { get; set; }
    }
}