namespace PostgreWebClient.Models;

public class QueryModel
{
    public string QueryText { get; set; } = null!;

    public Table? QueryResultTable { get; set; }
}