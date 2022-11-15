namespace PostgreWebClient.Models;

public class Table
{
    public List<string>? Columns { get; set; }
    public List<List<object>>? Rows { get; set; }

    public static Table Empty => new Table()
    {
        Columns = new List<string>(),
        Rows = new List<List<object>>()
    };

    public override bool Equals(object? obj)
    {
        if (obj is not Table table)
            return false;
        
        if (Columns?.Count != table.Columns?.Count)
            return false;

        if (Rows?.Count != table.Rows?.Count)
            return false;
        
        for (var i = 0; i < Rows?.Count; i++)
        {
            if (Rows?[i].Count != table.Rows?[i].Count)
                return false;
            if (Rows?[i].Count(element => table.Rows?[i].Contains(element) == false) != 0)
                return false;
        }

        return true;
    }
}