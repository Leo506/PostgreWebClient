namespace PostgreWebClient.Models;

public class QueryModel
{
    public string QueryText { get; set; } = null!;
    
    public List<int>? TableRows { get; set; }

    public List<object>? Rows { get; set; }

    public bool HasError { get; set; } = false;
    
    public string? ErrorText { get; set; }
}