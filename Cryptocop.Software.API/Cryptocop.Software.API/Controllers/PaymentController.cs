using System.Linq;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
  [Authorize]
  [Route("api/payments")]
  [ApiController]
  public class PaymentController : ControllerBase
  {
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
      _paymentService = paymentService;
    }

    [HttpGet]
    public IActionResult GetAllPaymentCards()
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      return Ok(_paymentService.GetStoredPaymentCards(email));
    }

    [HttpPost]
    public IActionResult AddPaymentCard([FromBody] PaymentCardInputModel inputModel)
    {
      // Check if inputModel is correct
      if (!ModelState.IsValid) { throw new ModelFormatException("Paymend card not properly formatted."); }

      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _paymentService.AddPaymentCard(email, inputModel);
      return StatusCode(201);
    }
  }
}