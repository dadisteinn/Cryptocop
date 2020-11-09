﻿using System.Linq;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
  [Authorize]
  [Route("api/cart")]
  [ApiController]
  public class ShoppingCartController : ControllerBase
  {
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
      _shoppingCartService = shoppingCartService;
    }

    [HttpGet]
    public IActionResult GetAllCartItems()
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      return Ok(_shoppingCartService.GetCartItems(email));
    }

    [HttpPost]
    public IActionResult AddCartItem([FromBody] ShoppingCartItemInputModel inputModel)
    {
      // Check if inputModel is correct
      if (!ModelState.IsValid) { throw new ModelFormatException("Cart item not properly formatted."); }

      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _shoppingCartService.AddCartItem(email, inputModel);
      return StatusCode(201);
    }

    [HttpDelete]
    [Route("{itemId:int}")]
    public IActionResult DeleteCartItem(int itemId)
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _shoppingCartService.RemoveCartItem(email, itemId);
      return NoContent();
    }

    [HttpPatch]
    [Route("{itemId:int}")]
    public IActionResult UpdateCartItem(int itemId)
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _shoppingCartService.RemoveCartItem(email, itemId);
      return NoContent();
    }

    [HttpDelete]
    public IActionResult DeleteCart()
    {
      // Get user email from claims
      var email = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
      _shoppingCartService.ClearCart(email);
      return NoContent();
    }
  }
}