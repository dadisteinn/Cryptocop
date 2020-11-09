using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Models.DTOs;

namespace Cryptocop.Software.API.Services.Implementations
{
  public class ShoppingCartService : IShoppingCartService
  {
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICryptoCurrencyService _cryptoCurrencyService;

    public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ICryptoCurrencyService cryptoCurrencyService)
    {
      _shoppingCartRepository = shoppingCartRepository;
      _cryptoCurrencyService = cryptoCurrencyService;
    }

    public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
    {
      return _shoppingCartRepository.GetCartItems(email);
    }

    public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
    {
      float priceInUsd = _cryptoCurrencyService.GetCryptocurrencyPrice(shoppingCartItemItem.ProductIdentifier).Result;
      _shoppingCartRepository.AddCartItem(email, shoppingCartItemItem, priceInUsd);
    }

    public void RemoveCartItem(string email, int id)
    {
      _shoppingCartRepository.RemoveCartItem(email, id);
    }

    public void UpdateCartItemQuantity(string email, int id, float quantity)
    {
      _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
    }

    public void ClearCart(string email)
    {
      _shoppingCartRepository.ClearCart(email);
    }
  }
}
