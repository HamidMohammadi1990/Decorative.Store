namespace Store.Infrastructure.Configurations;

public class RedisConfiguration
{
    public string Password { get; set; } = "";
    public bool AllowAdmin { get; set; } = false;
    public bool Ssl { get; set; }
    public int ConnectTimeout { get; set; } = 6000;
    public int ConnectRetry { get; set; } = 2;
    public int Database { get; set; }
    public List<RedisHost> Hosts { get; set; } = [new RedisHost()];
}
public class RedisHost
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 6379;
}