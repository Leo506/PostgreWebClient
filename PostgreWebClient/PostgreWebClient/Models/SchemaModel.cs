namespace PostgreWebClient.Models;

public class SchemaModel
{
    public string Name { get; set; } = null!;
    public List<string> Tables { get; set; } = null!;

    public List<string>? Views { get; set; }
}