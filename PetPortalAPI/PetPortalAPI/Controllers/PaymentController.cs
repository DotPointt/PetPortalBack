using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs.Requests;
using Swashbuckle.AspNetCore.Annotations;
using Yandex.Checkout.V3;

namespace PetPortalAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    [SwaggerOperation(Summary = "получить ссылку на оплату по параметрам")]
    [HttpGet()]
    public async Task<IActionResult> GetPaymentUrl(decimal amount, string currency)
    {
        Payment payment = await _paymentService.CreatePaymentAsync(amount, currency);
        
        _ = _paymentService.HandlePaymentAsync(payment.Id);
        
         // var task = Task.Run(async () =>
         // {
         //     while (!await _paymentService.HandlePaymentAsync(payment.Id)) //запустить таск на 5 минут
         //     {
         //         await Task.Delay(3000);
         //         Console.WriteLine("Waiting for payment");
         //     }
         //     return _paymentService.HandlePaymentAsync(payment.Id);
         // }); 
         // await task;
     
         return Ok(payment.Confirmation.ConfirmationUrl);
    }
    
}