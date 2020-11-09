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
  [Route("api/account")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;

    public AccountController(IAccountService accountService, ITokenService tokenService)
    {
      _accountService = accountService;
      _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public IActionResult RegisterUser([FromBody] RegisterInputModel inputModel)
    {
      // Check if inputModel is correct
      if (!ModelState.IsValid) { throw new ModelFormatException("Missing registration information."); }

      // Check if email is taken and create user if not
      var user = _accountService.CreateUser(inputModel);
      if (user == null) { return BadRequest("A user with this email already exists."); }
      return Created("signin", user);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("signin")]
    public IActionResult SignInUser([FromBody] LoginInputModel inputModel)
    {
      // Check if inputModel is correct
      if (!ModelState.IsValid) { throw new ModelFormatException("Missing login information."); }

      // Check if login information is valid
      var user = _accountService.AuthenticateUser(inputModel);
      if (user == null) { return Unauthorized("Invalid email or password."); }
      // Generate and return token
      return Ok(_tokenService.GenerateJwtToken(user));
    }

    [HttpGet]
    [Route("signout")]
    public IActionResult SignOutUser()
    {
      // Retrieve token id from claim and blacklist token  
      int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "tokenId").Value, out var tokenId);
      _accountService.Logout(tokenId);
      return NoContent();
    }
  }
}