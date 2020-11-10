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
  public class ShoppingCartRepository : IShoppingCartRepository
  {
    private readonly CryptocopDbContext _dbContext;

    public ShoppingCartRepository(CryptocopDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    private ShoppingCart CreateShoppingCart(int userId)
    {
      var entity = new ShoppingCart
      {
        UserId = userId
      };
      _dbContext.ShoppingCarts.Add(entity);
      _dbContext.SaveChanges();

      return entity;
    }

    public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
    {
      // Get user for his id
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get uset shoppingcart or create new cart if none exists
      var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
      if (cart == null) { cart = CreateShoppingCart(user.Id); }

      // Get all items from user cart
      var shoppingCartItems = _dbContext.ShoppingCartItems
                .Where(i => i.ShoppingCartId == cart.Id)
                .Select(i => new ShoppingCartItemDto
                {
                  Id = i.Id,
                  ProductIdentifier = i.ProductIdentifier,
                  Quantity = i.Quantity,
                  UnitPrice = i.UnitPrice,
                  TotalPrice = i.Quantity * i.UnitPrice
                })
                .ToList();

      return shoppingCartItems;
    }

    public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
    {
      // Get user for user id
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get user shoppingCart or create new if it doesnt exit
      var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
      if (cart == null) { cart = CreateShoppingCart(user.Id); }

      // Deal with nullable variable
      float quantity = 0.01F;
      if (shoppingCartItemItem.Quantity != null)
      {
        quantity = (float)shoppingCartItemItem.Quantity;
      }

      // Create new ShoppingCartItem
      var entity = new ShoppingCartItem
      {
        ShoppingCartId = cart.Id,
        ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
        Quantity = quantity,
        UnitPrice = priceInUsd
      };

      _dbContext.ShoppingCartItems.Add(entity);
      _dbContext.SaveChanges();
    }

    public void RemoveCartItem(string email, int id)
    {
      // Get user for user id
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get the item and 
      var cartItem = _dbContext.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
      if (cartItem == null) { throw new ResourceNotFoundException($"No item with id {id} was found."); }

      _dbContext.ShoppingCartItems.Remove(cartItem);
      _dbContext.SaveChanges();
    }

    public void UpdateCartItemQuantity(string email, int id, float quantity)
    {
      // Get user for user id
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get the item and update it
      var cartItem = _dbContext.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
      if (cartItem == null) { throw new ResourceNotFoundException($"No item with id {id} was found."); }

      cartItem.Quantity = quantity;

      _dbContext.ShoppingCartItems.Update(cartItem);
      _dbContext.SaveChanges();
    }

    public void ClearCart(string email)
    {
      // Get user for user id
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get user shoppingCart or create new if it doesnt exit
      var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
      if (cart == null) { return; }

      // Get cartitems, return if cart is empty
      var cartItems = _dbContext.ShoppingCartItems
          .Where(i => i.ShoppingCartId == cart.Id)
          .ToList();
      if (!cartItems.Any()) { return; }

      // Rmove cartitems from db
      _dbContext.ShoppingCartItems.RemoveRange(cartItems);
      _dbContext.SaveChanges();
    }

    public void DeleteCart(string email)
    {
      // Get user for user id
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get user shoppingCartId or return if none exists
      var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
      if (cart == null) { return; }

      // Clear the cart before deleting it
      ClearCart(email);

      //Remove cart
      _dbContext.ShoppingCarts.Remove(cart);
      _dbContext.SaveChanges();
    }
  }
}