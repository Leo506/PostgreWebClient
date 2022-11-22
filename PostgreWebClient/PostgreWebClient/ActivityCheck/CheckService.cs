using PostgreWebClient.Abstractions;

namespace PostgreWebClient.ActivityCheck;

public class CheckService : BackgroundService
{
    private readonly ConnectionActivityChecker _activityChecker;
    private readonly IConnectionService _connectionService;
    private readonly ActivityCheckSettings _settings;

    public CheckService(ConnectionActivityChecker activityChecker, IConnectionService connectionService,
        ActivityCheckSettings settings)
    {
        _activityChecker = activityChecker;
        _connectionService = connectionService;
        _settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _activityChecker.Check(_connectionService.Connections);
            await Task.Delay(_settings.TimeBeforeChecks, stoppingToken);
        }
    }
}