using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetPortalCore.Abstractions.Services;

namespace PetPortalApplication.Services;

/// <summary>
/// Раз в час переводит в архив проекты, у которых истёк срок приёма заявок.
/// </summary>
public class ProjectArchivingBackgroundService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProjectArchivingBackgroundService> _logger;

    public ProjectArchivingBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<ProjectArchivingBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // первый прогон сразу после старта, дальше — по таймеру
        await ArchiveExpiredAsync(stoppingToken);

        using var timer = new PeriodicTimer(Interval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ArchiveExpiredAsync(stoppingToken);
        }
    }

    private async Task ArchiveExpiredAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var projectsService = scope.ServiceProvider.GetRequiredService<IProjectsService>();

            var archivedCount = await projectsService.ArchiveExpired();

            if (archivedCount > 0)
            {
                _logger.LogInformation(
                    "В архив переведено проектов с истёкшим сроком приёма заявок: {Count}",
                    archivedCount);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // штатная остановка приложения
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка фонового переноса проектов в архив");
        }
    }
}
