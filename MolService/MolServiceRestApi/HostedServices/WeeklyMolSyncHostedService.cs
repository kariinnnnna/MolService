using MolServiceContracts.BindingModels;
using MolServiceContracts.BusinessLogicContracts;

namespace MolServiceRestApi.HostedServices
{
    public class WeeklyMolSyncHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeeklyMolSyncHostedService> _logger;

        public WeeklyMolSyncHostedService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<WeeklyMolSyncHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var enabled = _configuration.GetValue<bool>("WeeklySync:Enabled", true);

            if (!enabled)
            {
                _logger.LogInformation("Еженедельная синхронизация MolService отключена в настройках.");
                return;
            }

            var runOnStart = _configuration.GetValue<bool>("WeeklySync:RunOnStart", false);

            if (runOnStart)
            {
                await RunSynchronizationAsync(stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var nextRun = GetNextRunDateTime();
                var delay = nextRun - DateTime.Now;

                if (delay < TimeSpan.Zero)
                {
                    delay = TimeSpan.Zero;
                }

                _logger.LogInformation(
                    "Следующая автоматическая синхронизация MolService запланирована на {NextRun}",
                    nextRun);

                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                if (!stoppingToken.IsCancellationRequested)
                {
                    await RunSynchronizationAsync(stoppingToken);
                }
            }
        }

        private async Task RunSynchronizationAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Начата автоматическая синхронизация MolService.");

                using var scope = _scopeFactory.CreateScope();

                var coreClassroomImportLogic =
                    scope.ServiceProvider.GetRequiredService<ICoreClassroomImportLogic>();

                var oneCImportLogic =
                    scope.ServiceProvider.GetRequiredService<IOneCImportLogic>();

                await coreClassroomImportLogic.ImportClassroomsAsync();

                stoppingToken.ThrowIfCancellationRequested();

                var username = _configuration["OneC:AutoImportUsername"] ?? string.Empty;
                var password = _configuration["OneC:AutoImportPassword"] ?? string.Empty;

                var oneCResult = await oneCImportLogic.ImportFromOneCAsync(
                    new OneCImportBindingModel
                    {
                        Username = username,
                        Password = password
                    });

                _logger.LogInformation(
                    "Автоматическая синхронизация MolService завершена. " +
                    "Обработано: {ImportedCount}, создано: {CreatedCount}, обновлено: {UpdatedCount}, ошибок: {ErrorCount}",
                    oneCResult.ImportedCount,
                    oneCResult.CreatedCount,
                    oneCResult.UpdatedCount,
                    oneCResult.ErrorCount);

                if (oneCResult.Messages.Any())
                {
                    foreach (var message in oneCResult.Messages)
                    {
                        _logger.LogInformation("1С: {Message}", message);
                    }
                }

                if (oneCResult.Errors.Any())
                {
                    foreach (var error in oneCResult.Errors.Take(10))
                    {
                        _logger.LogWarning("Ошибка импорта 1С: {Error}", error);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Автоматическая синхронизация MolService остановлена.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка автоматической синхронизации MolService.");
            }
        }

        private DateTime GetNextRunDateTime()
        {
            var dayOfWeekName = _configuration["WeeklySync:DayOfWeek"] ?? "Monday";

            if (!Enum.TryParse<DayOfWeek>(dayOfWeekName, true, out var targetDayOfWeek))
            {
                targetDayOfWeek = DayOfWeek.Monday;
            }

            var hour = _configuration.GetValue<int>("WeeklySync:Hour", 3);
            var minute = _configuration.GetValue<int>("WeeklySync:Minute", 0);

            hour = Math.Clamp(hour, 0, 23);
            minute = Math.Clamp(minute, 0, 59);

            var now = DateTime.Now;

            var daysUntilTarget = ((int)targetDayOfWeek - (int)now.DayOfWeek + 7) % 7;

            var nextRun = now.Date
                .AddDays(daysUntilTarget)
                .AddHours(hour)
                .AddMinutes(minute);

            if (nextRun <= now)
            {
                nextRun = nextRun.AddDays(7);
            }

            return nextRun;
        }
    }
}