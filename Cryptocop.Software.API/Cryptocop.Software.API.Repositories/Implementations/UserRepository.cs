using System;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
  public class UserRepository : IUserRepository
  {
    private readonly CryptocopDbContext _dbContext;

    public UserRepository(CryptocopDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    private UserDto ToUserDto(User user, int tokenId)
    {
      return new UserDto
      {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        TokenId = tokenId
      };
    }

    public UserDto CreateUser(RegisterInputModel inputModel)
    {
      // Check if user with email already exists
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == inputModel.Email);
      if (user != null) { return null; }

      // Create new user
      var entity = new User
      {
        FullName = inputModel.FullName,
        Email = inputModel.Email,
        HashedPassword = HashingHelper.HashPassword(inputModel.Password)
      };
      _dbContext.Users.Add(entity);
      _dbContext.SaveChanges();

      // Create token
      var token = new JwtToken();
      _dbContext.JwtTokens.Add(token);
      _dbContext.SaveChanges();

      // Create dto to return
      return ToUserDto(entity, token.Id);
    }

    public UserDto AuthenticateUser(LoginInputModel loginInputModel)
    {
      // Authenticate login information
      var user = _dbContext.Users.FirstOrDefault(u =>
          u.Email == loginInputModel.Email &&
          u.HashedPassword == HashingHelper.HashPassword(loginInputModel.Password));
      if (user == null) { return null; }

      // Create token
      var token = new JwtToken();
      _dbContext.JwtTokens.Add(token);
      _dbContext.SaveChanges();

      // Create dto to return
      return ToUserDto(user, token.Id);
    }
  }
}