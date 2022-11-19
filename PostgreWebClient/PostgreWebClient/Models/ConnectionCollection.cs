using System.Collections;
using System.Data;

namespace PostgreWebClient.Models;

public class ConnectionCollection : IEnumerable<IDbConnection>
{
    public int Count => _connections.Count;

    private readonly Dictionary<string, IDbConnection> _connections = new();
    
    public void Add(string id, IDbConnection connection)
    {
        _connections.Add(id, connection);
        connection.Open();
    }

    public void RemoveById(string id)
    {
        var connection = _connections[id];
        connection.Close();
        _connections.Remove(id);
    }

    public IDbConnection this[string key]
    {
        get => _connections[key];
        set
        {
            _connections.Add(key, value);
            value.Open();
        }
    }
    
    public IEnumerator<IDbConnection> GetEnumerator()
    {
        return _connections.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}