using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQWeb.Watermark.Services;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.BackgroundServices
{
    public class ImageWatermarkProcessBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitMQClieantService;
        private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;
        private RabbitMQ.Client.IModel _channel;
        public ImageWatermarkProcessBackgroundService(RabbitMQClientService rabbitMQClieantService, ILogger<ImageWatermarkProcessBackgroundService> logger)
        {
            _rabbitMQClieantService = rabbitMQClieantService;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQClieantService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);
            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            Task.Delay(5000).Wait();

            try
            {
                var productImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", productImageCreatedEvent.ImageName);

                var siteName = "www.mysite.com";

                using var image = Image.FromFile(path);
                using var graphic = Graphics.FromImage(image);
                var font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold, GraphicsUnit.Pixel);
                var textSize = graphic.MeasureString(siteName, font);
                var color = Color.FromArgb(128, 255, 255, 255);
                var brush = new SolidBrush(color);
                var position = new Point(image.Width - ((int)textSize.Width + 30), image.Height - ((int)textSize.Height + 30));

                graphic.DrawString(siteName, font, brush, position);
                image.Save("wwwroot/Images/Watermark/" + productImageCreatedEvent.ImageName);


                image.Dispose();
                graphic.Dispose();

                _channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
