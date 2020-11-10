﻿using System.Collections.Generic;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Services.Interfaces
{
  public interface IShoppingCartService
  {
    IEnumerable<ShoppingCartItemDto> GetCartItems(string email);
    void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem);
    void RemoveCartItem(string email, int id);
    void UpdateCartItemQuantity(string email, int id, float quantity);
    void ClearCart(string email);
  }
}