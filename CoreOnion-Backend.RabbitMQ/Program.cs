using CoreOnion_Backend.Application.Interfaces.MailService;
using CoreOnion_Backend.Infrastructure.Services;
using CoreOnion_Backend.RabbitMQ.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreOnion_Backend.RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IMailService, MailService>();
                    services.AddSingleton<RabbitMQConsumer>();
                })
                .Build();

            var consumer = host.Services.GetRequiredService<RabbitMQConsumer>();

            consumer.StartListening();
        }
    }
}
