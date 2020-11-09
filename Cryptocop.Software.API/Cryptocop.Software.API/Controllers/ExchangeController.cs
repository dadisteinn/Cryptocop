using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
  [Authorize]
  [Route("api/exchanges")]
  [ApiController]
  public class ExchangeController : ControllerBase
  {
    public readonly IExchangeService _exchangeService;

    public ExchangeController(IExchangeService exchangeService)
    {
      _exchangeService = exchangeService;
    }

    public IActionResult GetAllExchanges([FromQuery] int pageNumber = 1)
    {
      return Ok(_exchangeService.GetExchanges(pageNumber).Result);
    }
  }
}