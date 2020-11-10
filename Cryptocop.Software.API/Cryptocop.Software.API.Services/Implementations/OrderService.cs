using System.Collections.Generic;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cryptocop.Software.API.Services.Implementations
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IQueueService _queueService;
    private readonly string _routingKey;

    public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, IQueueService queueService, IConfiguration configuration)
    {
      _orderRepository = orderRepository;
      _shoppingCartRepository = shoppingCartRepository;
      _routingKey = configuration.GetSection("MessageBroker").GetSection("RoutingKey").Value;
      _queueService = queueService;
    }

    public IEnumerable<OrderDto> GetOrders(string email)
    {
      return _orderRepository.GetOrders(email);
    }

    public void CreateNewOrder(string email, OrderInputModel order)
    {
      var dto = _orderRepository.CreateNewOrder(email, order);
      _shoppingCartRepository.DeleteCart(email);
      _queueService.PublishMessage(_routingKey, dto);
    }
  }
}