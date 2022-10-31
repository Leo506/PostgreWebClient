using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface ICommandExecutor
{
    Table Execute();
}