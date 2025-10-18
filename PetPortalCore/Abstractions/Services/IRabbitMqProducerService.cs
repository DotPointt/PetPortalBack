namespace PetPortalCore.Abstractions.Services;

public interface IRabbitMqProducerService
{

    public Task PublishAsync<T>(T message, string routingKey);

}