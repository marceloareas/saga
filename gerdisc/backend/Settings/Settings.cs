namespace saga.Settings
{
    public class AppSettings : ISettings
    {
        public IConfigurationRoot config { get; }

        public string SinginKey => config[nameof(SinginKey)] ?? "";
        public string postgresPort => config[nameof(postgresPort)] ?? "";
        public string PostgresServer => config[nameof(PostgresServer)] ?? "";
        public string PostgresUser => config[nameof(PostgresUser)] ?? "";
        public string PostgresPassword => config[nameof(PostgresPassword)] ?? "";
        public string PostgresDb => config[nameof(PostgresDb)] ?? "";

        public EmailSettings EmailSettings => new EmailSettings(config);

        public AppSettings()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
