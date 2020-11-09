using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
  [Authorize]
  [Route("api/orders")]
  [ApiController]
  public class OrderController : ControllerBase
  {
    private readonly IOrderService _orderservice;

    public OrderController(IOrderService orderservice)
    {
      _orderservice = orderservice;
    }

    [HttpGet]
    public IActionResult GetAllOrders()
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      return Ok(_orderservice.GetOrders(email));
    }

    [HttpPost]
    public IActionResult AddOrder([FromBody] OrderInputModel inputModel)
    {
      // Check if inputModel is correct
      if (!ModelState.IsValid) { throw new ModelFormatException("Order not properly formatted."); }

      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _orderservice.CreateNewOrder(email, inputModel);
      return StatusCode(201);
    }

    // TODO: remove this
    [HttpPost]
    [Route("test")]
    public IActionResult publishIncomingDto([FromBody] OrderDto dtoModel)
    {
      if (!ModelState.IsValid) { throw new ModelFormatException("Order not properly formatted."); }

      _orderservice.publishIncomingDto(dtoModel);
      return Ok();
    }
  }
}