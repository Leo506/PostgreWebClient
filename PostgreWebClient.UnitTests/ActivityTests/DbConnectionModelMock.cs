using System.Data;
using PostgreWebClient.Models;

namespace PostgreWebClient.UnitTests.ActivityTests;

public class DbConnectionModelMock : DbConnectionModel
{
    public DbConnectionModelMock(IDbConnection connection, DateTime lastActivity) : base(connection)
    {
        LastActivity = lastActivity;
    }

    public override void Open()
    {
        
    }
}