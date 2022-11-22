using System.Collections;
using System.Data;

namespace PostgreWebClient.Models;

public class ConnectionCollection : IEnumerable<KeyValuePair<string, IDbConnection>>
{
    public int Count => _connections.Count;

    private readonly Dictionary<string, IDbConnection> _connections = new();
    
    public void Add(string id, IDbConnection connection)
    {
        _connections.Add(id, connection);
        connection.Open();
    }

    public void Remove(string id)
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

    public bool ContainsKey(string key) => _connections.ContainsKey(key);
    
    public IEnumerator<KeyValuePair<string, IDbConnection>> GetEnumerator()
    {
        return _connections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}