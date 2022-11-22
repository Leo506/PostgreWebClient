using System.Data;

namespace PostgreWebClient.Models;

public class DbConnectionModel : IDbConnection
{
    public DateTime LastActivity { get; protected set; }

    #region IDbConnection implementation
    public int ConnectionTimeout => Connection.ConnectionTimeout;

    public string Database => Connection.Database;

    public ConnectionState State => Connection.State;
    #endregion
    
    public IDbConnection Connection { get; private set; }
    
    public DbConnectionModel(IDbConnection connection)
    {
        Connection = connection;
        LastActivity = DateTime.UtcNow;
    }

    #region IDbConnection implementation
    public void Dispose()
    {
        Connection.Close();
        Connection.Dispose();
    }

    public IDbTransaction BeginTransaction()
    {
        LastActivity = DateTime.UtcNow;
        return Connection.BeginTransaction();
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        LastActivity = DateTime.UtcNow;
        return Connection.BeginTransaction(il);
    }

    public void ChangeDatabase(string databaseName)
    {
        LastActivity = DateTime.UtcNow;
        Connection.ChangeDatabase(databaseName);
    }

    public void Close()
    {
        Connection.Close();
    }

    public IDbCommand CreateCommand()
    {
        LastActivity = DateTime.UtcNow;
        return Connection.CreateCommand();
    }

    public virtual void Open()
    {
        LastActivity = DateTime.UtcNow;
        Connection.Open();
    }

    public string ConnectionString
    {
        get => Connection.ConnectionString;
        set => Connection.ConnectionString = value;
    }
    #endregion
    
}