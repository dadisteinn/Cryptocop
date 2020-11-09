using System.Collections.Generic;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Repositories.Interfaces
{
  public interface IAddressRepository
  {
    IEnumerable<AddressDto> GetAllAddresses(string email);
    void AddAddress(string email, AddressInputModel address);
    void DeleteAddress(string email, int addressId);
  }
}