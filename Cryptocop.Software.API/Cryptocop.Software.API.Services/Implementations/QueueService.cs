using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Cryptocop.Software.API.Services.Implementations
{
  public class QueueService : IQueueService, IDisposable
  {
    private IModel _channel;
    private IConfiguration Configuration;

    private byte[] ConvertJsonToBytes(object obj) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

    public QueueService(IConfiguration configuration)
    {
      Configuration = configuration;
      try
      {
        var messageBrokerSection = configuration.GetSection("MessageBroker");
        var factory = new ConnectionFactory();
        factory.Uri = new System.Uri(messageBrokerSection.GetSection("ConnectionString").Value);
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void PublishMessage(string routingKey, object body)
    {
      var messageBrokerSection = Configuration.GetSection("MessageBroker");
      _channel.BasicPublish(
        exchange: messageBrokerSection.GetSection("ExchangeName").Value,
        routingKey,
        mandatory: true,
        basicProperties: null,
        body: ConvertJsonToBytes(body)
      );
    }

    public void Dispose()
    {
      _channel.Close();
    }

  }
}