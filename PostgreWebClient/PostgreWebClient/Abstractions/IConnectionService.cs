namespace PostgreWebClient.Abstractions;

public interface IConnectionService
{
    void Connect(string connectionString);
}