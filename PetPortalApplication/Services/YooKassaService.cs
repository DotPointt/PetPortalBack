 using Amazon.Runtime.Internal;
 using Microsoft.Extensions.Hosting;
 using Microsoft.Extensions.Options;
 using PetPortalCore.Abstractions.Services;
 using PetPortalCore.Configs;
 using Yandex.Checkout.V3;

// public class PaymentResult
// {
//     public bool RequiresRedirect { get; set; }
//     public string RedirectUrl { get; set; }
//     public string ErrorMessage { get; set; }
// }
 
 

namespace PetPortalApplication.Services
{
    public class YooKassaService : IPaymentService //, IHostedService
    {
        private AsyncClient _asyncClient { get; set; }
        private Timer? _timer = null;
        private readonly YooKassaConfig _shopConfig;
        
        public YooKassaService(IOptions<YooKassaConfig> options)
        {
            _shopConfig = options.Value;
            
            var client = new Yandex.Checkout.V3.Client(
                shopId: _shopConfig.ShopId, 
                secretKey: _shopConfig.TestMagazineSecretKey);

            _asyncClient = client.MakeAsync();
        }

        public async Task<Payment> CreatePaymentAsync(decimal аmount, string currency)
        {
            var newPayment = new NewPayment
            {
                Amount = new Amount { Value = аmount, Currency = currency },
                Confirmation = new Confirmation 
                { 
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = "http://petportal/projects"
                }
            };

            var payment = await _asyncClient.CreatePaymentAsync(newPayment);
            return payment;
        }

        public async Task HandlePaymentAsync(string id)
        {
            int maxAttempts = 100; // Максимальное количество попыток (например, 5 минут при задержке 3 секунды)
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                // Проверяем статус оплаты
                Payment payment = await _asyncClient.GetPaymentAsync(id);
                bool isPaid = payment.Paid;

                if (isPaid)
                {
                    // Оплата успешна, выполняем необходимые действия
                    Console.WriteLine("Payment successful!");
                    break;
                }

                // Ждем перед следующей проверкой
                await Task.Delay(3000);
                attempt++;
                Console.WriteLine($"Attempt {attempt}: Waiting for payment...");
            }

            if (attempt == maxAttempts)
            {
                // Если время истекло, обрабатываем таймаут
                Console.WriteLine("Payment timeout.");
            }
        }



    }
}
