using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Configs;
using PetPortalCore.Models;
using PetPortalDAL;
using PetPortalDAL.Entities;
using Yandex.Checkout.V3;

namespace PetPortalApplication.Services;

public class YooKassaService : IPaymentService
{
    private readonly YooKassaConfig _shopConfig;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AsyncClient _asyncClient;

    public YooKassaService(IOptions<YooKassaConfig> options, IServiceScopeFactory scopeFactory)
    {
        _shopConfig = options.Value;
        _scopeFactory = scopeFactory;

        var client = new Client(
            shopId: _shopConfig.ShopId,
            secretKey: _shopConfig.TestMagazineSecretKey);

        _asyncClient = client.MakeAsync();
    }

    public async Task<string> CreatePlacementPaymentAsync(Guid projectId, Guid userId)
    {
        var amount = _shopConfig.PlacementAmount;
        var currency = string.IsNullOrWhiteSpace(_shopConfig.PlacementCurrency)
            ? "RUB"
            : _shopConfig.PlacementCurrency;

        var newPayment = new NewPayment
        {
            Amount = new Amount { Value = amount, Currency = currency },
            Capture = true,
            Description = $"Размещение проекта {projectId}",
            Confirmation = new Confirmation
            {
                Type = ConfirmationType.Redirect,
                ReturnUrl = _shopConfig.ReturnUrl
            },
            Metadata = new Dictionary<string, string>
            {
                ["projectId"] = projectId.ToString(),
                ["userId"] = userId.ToString()
            }
        };

        var payment = await _asyncClient.CreatePaymentAsync(newPayment);

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<PetPortalDbContext>();
            db.PlacementPayments.Add(new PlacementPaymentEntity
            {
                Id = Guid.NewGuid(),
                YooKassaPaymentId = payment.Id,
                ProjectId = projectId,
                UserId = userId,
                Amount = amount,
                Currency = currency,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        // Background poll (scoped-safe via scope factory inside HandlePaymentAsync)
        _ = HandlePaymentAsync(payment.Id);

        return payment.Confirmation.ConfirmationUrl;
    }

    public async Task HandlePaymentAsync(string yooKassaPaymentId)
    {
        var maxAttempts = 100;
        var attempt = 0;

        while (attempt < maxAttempts)
        {
            if (await ConfirmPaymentAndPublishAsync(yooKassaPaymentId))
                return;

            await Task.Delay(3000);
            attempt++;
        }

        Console.WriteLine($"Payment timeout for {yooKassaPaymentId}");
    }

    public async Task<bool> ConfirmPaymentAndPublishAsync(string yooKassaPaymentId)
    {
        var payment = await _asyncClient.GetPaymentAsync(yooKassaPaymentId);
        if (!payment.Paid)
            return false;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PetPortalDbContext>();

        var record = await db.PlacementPayments
            .FirstOrDefaultAsync(p => p.YooKassaPaymentId == yooKassaPaymentId);

        if (record == null)
        {
            // Fallback: metadata from YooKassa
            if (payment.Metadata != null &&
                payment.Metadata.TryGetValue("projectId", out var projectIdStr) &&
                Guid.TryParse(projectIdStr, out var metaProjectId))
            {
                await PublishProjectAsync(db, metaProjectId);
                return true;
            }

            Console.WriteLine($"Payment successful but no local record: {yooKassaPaymentId}");
            return true;
        }

        if (record.Status == "succeeded")
            return true;

        record.Status = "succeeded";
        record.PaidAt = DateTime.UtcNow;
        await PublishProjectAsync(db, record.ProjectId);
        await db.SaveChangesAsync();

        Console.WriteLine($"Payment successful, project {record.ProjectId} published");
        return true;
    }

    private static async Task PublishProjectAsync(PetPortalDbContext db, Guid projectId)
    {
        await db.Projects
            .Where(p => p.Id == projectId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.StateOfProject, StateOfProject.Open));
    }
}
