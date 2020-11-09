﻿using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
  public class AccountService : IAccountService
  {
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;

    public AccountService(IUserRepository userRepository, ITokenRepository tokenRepository)
    {
      _userRepository = userRepository;
      _tokenRepository = tokenRepository;
    }

    public UserDto CreateUser(RegisterInputModel inputModel)
    {
      var user = _userRepository.CreateUser(inputModel);
      if (user == null) { throw new InvalidRegistrationException(); }
      return user;
    }

    public UserDto AuthenticateUser(LoginInputModel loginInputModel)
    {
      var user = _userRepository.AuthenticateUser(loginInputModel);
      if (user == null) { throw new InvalidSignInException(); }
      return user;
    }

    public void Logout(int tokenId)
    {
      _tokenRepository.VoidToken(tokenId);
    }
  }
}