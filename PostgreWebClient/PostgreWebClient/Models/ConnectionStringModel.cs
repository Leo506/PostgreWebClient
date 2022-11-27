using System.ComponentModel;
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
    
    [Required]
    public bool UseDockerContainer { get; set; }
    
    public string? DockerContainerName { get; set; }

    public string ToConnectionString()
    {
        return
            $"User id={UserId};Password={Password};Host={(UseDockerContainer ? DockerContainerName : Host)};" +
            $"Port={Port};Database={Database}";
    }
}