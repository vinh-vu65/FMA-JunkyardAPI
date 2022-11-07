namespace JunkyardWebApp.API.Configurations;

public static class DbConfiguration
{
    public static string GetDbConnectionString()
    {
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var username = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        
        return $"Database={dbName};Host={dbHost};Username={username};Password={password};Port={port}";
    }
}