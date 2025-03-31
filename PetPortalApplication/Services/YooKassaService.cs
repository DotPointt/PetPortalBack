using Amazon.Runtime.Internal;
using PetPortalCore.Abstractions.Services;
using Yandex.Checkout.V3;


namespace PetPortalApplication.Services
{
    public class YooKassaService : ICashierService
    {
        public string GetPayUrl( string shopId, string secretKey)
        {
            var client = new Yandex.Checkout.V3.Client(
                shopId: shopId,
                secretKey: secretKey);

            AsyncClient asyncClient = client.MakeAsync();

            // 1. Создайте платеж и получите ссылку для оплаты
            var newPayment = new NewPayment
            {
                Amount = new Amount { Value = 100.00m, Currency = "RUB" },
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = "http://myshop.ru/thankyou"
                }
            };
            Payment payment = client.CreatePayment(newPayment);

            // 2. Перенаправьте пользователя на страницу оплаты
            string url = payment.Confirmation.ConfirmationUrl;
        }


    }
}
