using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PetPortalAPI.Options;
using PetPortalCore.Abstractions.Services;
using RabbitMQ.Client;

namespace PetPortalApplication.Services;

public class RabbitMqProducerService : IRabbitMqProducerService, IDisposable
{
    private readonly IConnection _connection;
    private readonly Lazy<Task<IConnection>> _connectionFactory;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public RabbitMqProducerService(IOptions<RabbitMqOptions> options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            Port = options.Value.Port,
            UserName = options.Value.UserName,
            Password = options.Value.Password,
            VirtualHost = options.Value.VirtualHost
        };
        _connectionFactory = new Lazy<Task<IConnection>>(() => factory.CreateConnectionAsync());
    }

    private async Task<IConnection> GetConnectionAsync()
    {
        return await _connectionFactory.Value;
    }
    
    public async Task PublishAsync<T>(T message, string routingKey)
    {
        var connection = await GetConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        try
        {
            var body = JsonSerializer.SerializeToUtf8Bytes(message, _jsonOptions);
            await channel.BasicPublishAsync(
                exchange: "basicExchange",
                routingKey: routingKey,
                body: body);
        }
        finally
        {
            if (channel is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();
            else
                channel?.Dispose();
        }
    }

    public void Dispose()
    {
        if (_connectionFactory.IsValueCreated)
        {
            var connectionTask = _connectionFactory.Value;
            if (connectionTask.IsCompletedSuccessfully)
            {
                connectionTask.Result?.Dispose();
            }
        }
    }
}