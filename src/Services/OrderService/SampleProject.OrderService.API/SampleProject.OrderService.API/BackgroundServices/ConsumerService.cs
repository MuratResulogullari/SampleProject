namespace SampleProject.OrderService.API.BackgroundServices;


using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


public class ConsumerService : BackgroundService
{
  private readonly ILogger<ConsumerService> _logger;
  private readonly IConfiguration _config;
  private readonly IServiceProvider _serviceProvider;
  private IConnection? _messageConnection;
  private IModel? _messageChannel;

  public ConsumerService(ILogger<ConsumerService> logger, IConfiguration config, IServiceProvider serviceProvider, IConnection? messageConnection)
  {
    _logger = logger;
    _config = config;
    _serviceProvider = serviceProvider;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    string queueName = "CatalogServiceQueue";

    _messageConnection = _serviceProvider.GetService<IConnection>();

    _messageChannel = _messageConnection!.CreateModel();
    _messageChannel.QueueDeclare(queue: queueName,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    var consumer = new EventingBasicConsumer(_messageChannel);
    consumer.Received += ProcessMessageAsync;

    _messageChannel.BasicConsume(queue: queueName,
        autoAck: true,
        consumer: consumer);

    return Task.CompletedTask;
  }

  public override async Task StopAsync(CancellationToken cancellationToken)
  {
    await base.StopAsync(cancellationToken);

    _messageChannel?.Dispose();
  }

  private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
  {
    var messageJson = Encoding.UTF8.GetString(args.Body.ToArray());
    _logger.LogInformation($"Received message: {messageJson}");

    try
    {
      // Deserialize the JSON string to an object of type CatalogCountMessage
      var catalogCountMessage = JsonSerializer.Deserialize<CatalogCountMessage>(messageJson);
      if (catalogCountMessage != null)
      {
        // Process the deserialized object
        _logger.LogInformation($"Deserialized CatalogCount: {catalogCountMessage.CatalogCount}");
      }
    }
    catch (JsonException ex)
    {
      _logger.LogError($"Failed to deserialize message: {ex.Message}");
    }

    var message = args.Body;
  }
  public class CatalogCountMessage
  {
    public int CatalogCount { get; set; }
  }
}