namespace PostgreWebClient.Models;

public class SchemaModel
{
    public string SchemaName { get; set; } = null!;
    public List<string> Tables { get; set; } = null!;
}