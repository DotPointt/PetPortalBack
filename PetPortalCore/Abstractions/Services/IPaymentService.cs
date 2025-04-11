namespace PetPortalCore.Abstractions.Services;
using Yandex.Checkout.V3; //не хочу писать свои моделькиии пускай пока будет яндекс тут
public interface IPaymentService
{
    public Task<Payment> CreatePaymentAsync(decimal аmount, string currency);
    public Task HandlePaymentAsync(string id);
}