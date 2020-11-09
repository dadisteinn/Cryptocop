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
  public class OrderRepository : IOrderRepository
  {
    private readonly CryptocopDbContext _dbContext;

    public OrderRepository(CryptocopDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    private static OrderDto ToOrderDto(Order order)
    {
      return new OrderDto
      {
        Id = order.Id,
        Email = order.Email,
        FullName = order.FullName,
        StreetName = order.StreetName,
        HouseNumber = order.HouseNumber,
        ZipCode = order.ZipCode,
        Country = order.Country,
        City = order.City,
        CardholderName = order.CardholderName,
        CreditCard = order.MaskedCreditCard,
        OrderDate = order.OrderDate.ToString("dd.MM.yyyy"),
        TotalPrice = order.TotalPrice,
        OrderItems = new List<OrderItemDto>()
      };
    }

    private List<OrderItemDto> GetOrderItems(int orderId)
    {
      // Check if order exists
      var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
      if (order == null) { throw new ResourceNotFoundException("Order not found."); }

      // Return all order items
      return _dbContext.OrderItems
                .Where(o => o.OrderId == orderId)
                .Select(o => new OrderItemDto
                {
                  Id = o.Id,
                  ProductIdentifier = o.ProducrIdentifier,
                  Quantity = o.Quantity,
                  UnitPrice = o.UnitPrice,
                  TotalPrice = o.TotalPrice
                }).ToList();
    }

    public IEnumerable<OrderDto> GetOrders(string email)
    {
      // Get all order assigned to given email
      var orders = _dbContext.Orders
                    .Where(o => o.Email == email)
                    .Select(o => ToOrderDto(o))
                    .ToList();
      if (orders == null) { return new List<OrderDto>(); }

      // Fill orderItems
      foreach (var order in orders)
      {
        order.OrderItems = GetOrderItems(order.Id);
      }

      return orders;
    }

    public OrderDto CreateNewOrder(string email, OrderInputModel order)
    {
      // Get user information
      var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
      if (user == null) { throw new ResourceMissingException("No user was found with this email."); }

      // Get user address
      var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == order.AddressId);
      if (address == null) { throw new ResourceMissingException("No address has been registered for this user."); }

      // Get user card information
      var paymentCard = _dbContext.PaymentCards.FirstOrDefault(c => c.Id == order.PaymentCardId);
      if (paymentCard == null) { throw new ResourceMissingException("No Payment card has been registered for this user."); }

      // Get user shoppingcart or create new cart if none exists
      var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
      if (cart == null) { throw new ResourceMissingException("Shopping cart is empty."); }

      // Create order items from cart items
      var orderItems = _dbContext.ShoppingCartItems
                .Where(i => i.ShoppingCartId == cart.Id)
                .Select(i => new OrderItem
                {
                  Id = i.Id,
                  ProducrIdentifier = i.ProductIdentifier,
                  Quantity = i.Quantity,
                  UnitPrice = i.UnitPrice,
                  TotalPrice = i.Quantity * i.UnitPrice
                })
                .ToList();
      if (!orderItems.Any()) { throw new ResourceMissingException("Shopping cart is empty."); }

      // Calculate total order price
      var totalPrice = 0F;
      foreach (var item in orderItems)
      {
        totalPrice += item.TotalPrice;
      }

      var maskedCardNumber = "************" + paymentCard.CardNumber.Substring(paymentCard.CardNumber.Length - 4);

      // Create and save order
      var entity = new Order
      {
        Email = user.Email,
        FullName = user.FullName,
        StreetName = address.StreetName,
        HouseNumber = address.HouseNumber,
        ZipCode = address.ZipCode,
        Country = address.Country,
        City = address.City,
        CardholderName = paymentCard.CardholderName,
        MaskedCreditCard = maskedCardNumber,
        OrderDate = DateTime.Now,
        TotalPrice = totalPrice
      };
      _dbContext.Orders.Add(entity);
      _dbContext.SaveChanges();

      // Save order items
      AddOrderItems(entity.Id, orderItems);

      // Create and return order DTO
      var dto = ToOrderDto(entity);
      dto.CreditCard = paymentCard.CardNumber;
      dto.OrderItems = GetOrderItems(dto.Id);
      return dto;
    }

    public void AddOrderItems(int orderId, List<OrderItem> orderItems)
    {
      // Add order id to each item and save it
      foreach (var item in orderItems)
      {
        item.OrderId = orderId;
        _dbContext.OrderItems.Add(item);
      }
      _dbContext.SaveChanges();
    }
  }
}