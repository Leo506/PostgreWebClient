using System.Data;
using PostgreWebClient.Models;

namespace PostgreWebClient.ActivityCheck;

public class ConnectionActivityChecker
{
    private readonly ActivityCheckSettings _settings;

    public ConnectionActivityChecker(ActivityCheckSettings settings)
    {
        _settings = settings;
    }

    public void Check(ConnectionCollection collection)
    {
        var toDelete = new List<string>();
        foreach (var connection in collection)
        {
            var connectionModel = connection.Value as DbConnectionModel;
            if ((DateTime.UtcNow - connectionModel!.LastActivity) >= _settings.TimeBeforeClose)
                toDelete.Add(connection.Key);
        }
        
        foreach (var id in toDelete)
        {
            collection.Remove(id);
        }
    }
}