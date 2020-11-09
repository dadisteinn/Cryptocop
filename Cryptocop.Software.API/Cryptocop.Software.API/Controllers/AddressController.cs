using System;
using System.Linq;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
  [Authorize]
  [Route("api/addresses")]
  [ApiController]
  public class AddressController : ControllerBase
  {
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
      _addressService = addressService;
    }

    [HttpGet]
    public IActionResult GetAllAddresses()
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      return Ok(_addressService.GetAllAddresses(email));
    }

    [HttpPost]
    public IActionResult AddAddress([FromBody] AddressInputModel inputModel)
    {
      // Check if inputModel is correct
      if (!ModelState.IsValid) { throw new ModelFormatException("Address not properly formatted."); }

      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _addressService.AddAddress(email, inputModel);
      return StatusCode(201);
    }

    [HttpDelete]
    [Route("{addressId:int}")]
    public IActionResult DeleteAddress(int addressId)
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _addressService.DeleteAddress(email, addressId);
      return NoContent();
    }
  }
}