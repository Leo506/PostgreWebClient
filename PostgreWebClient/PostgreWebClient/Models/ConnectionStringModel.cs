using System.ComponentModel.DataAnnotations;

namespace PostgreWebClient.Models;

public class ConnectionStringModel
{
    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required] public string Host { get; set; } = "localhost";

    [Required] public int Port { get; set; } = 5432;

    [Required] public string Database { get; set; } = null!;

    public string ToConnectionString()
    {
        return $"User id={UserId};Password={Password};Host={Host};Port={Port};Database={Database}";
    }
}