using System.Data;

namespace PostgreWebClient.Models;

public class DbConnectionModel : IDbConnection
{
    public DateTime LastActivity { get; protected set; }

    #region IDbConnection implementation
    public int ConnectionTimeout => _connection.ConnectionTimeout;

    public string Database => _connection.Database;

    public ConnectionState State => _connection.State;
    #endregion
    
    private readonly IDbConnection _connection;
    
    public DbConnectionModel(IDbConnection connection)
    {
        _connection = connection;
        _connection.Open();
        LastActivity = DateTime.UtcNow;
    }

    #region IDbConnection implementation
    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }

    public IDbTransaction BeginTransaction()
    {
        LastActivity = DateTime.UtcNow;
        return _connection.BeginTransaction();
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        LastActivity = DateTime.UtcNow;
        return _connection.BeginTransaction(il);
    }

    public void ChangeDatabase(string databaseName)
    {
        LastActivity = DateTime.UtcNow;
        _connection.ChangeDatabase(databaseName);
    }

    public void Close()
    {
        _connection.Close();
    }

    public IDbCommand CreateCommand()
    {
        LastActivity = DateTime.UtcNow;
        return _connection.CreateCommand();
    }

    public virtual void Open()
    {
        LastActivity = DateTime.UtcNow;
        _connection.Open();
    }

    public string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }
    #endregion
    
}