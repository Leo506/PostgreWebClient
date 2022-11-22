namespace PostgreWebClient.Models;

public partial class Table
{
    public static Table Empty => new Table()
    {
        Columns = new List<string>(),
        Rows = new List<List<object>>()
    };

    public static Table SuccessResult(string query) => new Table()
    {
        Columns = new List<string>() { "Query", "Result" },
        Rows = new List<List<object>>()
        {
            new() { query, "Success" }
        }
    };

    public static Table ErrorResult(string query, string message) => new Table()
    {
        Columns = new List<string>() { "Query", "Result", "Reason" },
        Rows = new List<List<object>>()
        {
            new() { query, "Failed", message }
        }
    };
}