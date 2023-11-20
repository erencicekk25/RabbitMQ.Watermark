

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.Services
{
    public class RabbidMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbidMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabbitMQClientService.Connect();
            var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark,  basicProperties: properties, body: bodyByte);
        }
    }
}
