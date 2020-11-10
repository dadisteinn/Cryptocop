using System;
using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
  public class AddressRepository : IAddressRepository
  {
    private readonly CryptocopDbContext _dbContext;

    public AddressRepository(CryptocopDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public IEnumerable<AddressDto> GetAllAddresses(string email)
    {
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      return _dbContext.Addresses
                .Where(a => a.UserId == user.Id)
                .Select(a => new AddressDto
                {
                  Id = a.Id,
                  StreetName = a.StreetName,
                  HouseNumber = a.HouseNumber,
                  ZipCode = a.ZipCode,
                  Country = a.Country,
                  City = a.City
                }).ToList();
    }

    public void AddAddress(string email, AddressInputModel address)
    {
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      var entity = new Address
      {
        UserId = user.Id,
        StreetName = address.StreetName,
        HouseNumber = address.HouseNumber,
        ZipCode = address.ZipCode,
        Country = address.Country,
        City = address.City
      };
      _dbContext.Addresses.Add(entity);
      _dbContext.SaveChanges();
    }

    public void DeleteAddress(string email, int addressId)
    {
      // Check if user exists
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get address or throw error if it doesnt exist
      var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == addressId);
      if (address == null) { throw new ResourceNotFoundException($"No address with id {addressId} was found."); }

      // Remove the address
      _dbContext.Addresses.Remove(address);
      _dbContext.SaveChanges();
    }
  }
}