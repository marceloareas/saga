namespace saga.Settings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }

        public EmailSettings(IConfigurationRoot config)
        {
            SmtpServer = config["EmailSettings:SmtpServer"] ?? "";
            SmtpPort = int.Parse(config["EmailSettings:SmtpPort"] ?? "0");
            Username = config["EmailSettings:Username"] ?? "";
            Password = config["EmailSettings:Password"] ?? "";
            SenderEmail = config["EmailSettings:SenderEmail"] ?? "";
        }
    }
}
