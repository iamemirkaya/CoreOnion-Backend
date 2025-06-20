using CoreOnion_Backend.Application.Interfaces.MailService;
using CoreOnion_Backend.RabbitMQ.model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CoreOnion_Backend.RabbitMQ.Services
{
    public class RabbitMQConsumer
    {
        private readonly string _hostName = "aaaaaaaaaaaaaaaaaaa";
        private readonly string _queueName = "mailQueue";
        private readonly IMailService _mailService;

        public RabbitMQConsumer(IMailService mailService)
        {
            _mailService = mailService;
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_hostName) };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    try
                    {
                        var mailMessage = JsonConvert.DeserializeObject<MailSendMessage>(message);
                        if (mailMessage != null && (!string.IsNullOrWhiteSpace(mailMessage.To) || (mailMessage.Tos != null && mailMessage.Tos.Length > 0)))
                        {
                            await ProcessMailMessage(mailMessage);
                            return;
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        var passwordResetMessage = JsonConvert.DeserializeObject<PasswordResetMessage>(message);
                        if (passwordResetMessage != null && !string.IsNullOrWhiteSpace(passwordResetMessage.Email))
                        {

                            await ProcessPasswordResetMessage(passwordResetMessage);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Password reset mesajı işlenirken hata: {ex.Message}");
                    }

                    Console.WriteLine("Bilinmeyen mesaj formatı.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mesaj işlenirken genel hata: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("[*] Waiting for messages.");
            Console.ReadLine();
        }

        private async Task ProcessMailMessage(MailSendMessage mailMessage)
        {
            if (!string.IsNullOrWhiteSpace(mailMessage.To))
            {
                await _mailService.SendMailAsync(
                    mailMessage.To,
                    mailMessage.Subject,
                    mailMessage.Body,
                    mailMessage.IsBodyHtml);
            }
            else if (mailMessage.Tos != null && mailMessage.Tos.Length > 0)
            {
                await _mailService.SendMailAsync(
                    mailMessage.Tos,
                    mailMessage.Subject,
                    mailMessage.Body,
                    mailMessage.IsBodyHtml);
            }

            Console.WriteLine("Normal mail sent successfully.");
        }

        private async Task ProcessPasswordResetMessage(PasswordResetMessage passwordResetMessage)
        {
            await _mailService.SendPasswordResetMailAsync(
                passwordResetMessage.Email,
                passwordResetMessage.UserId,
                passwordResetMessage.ResetToken);

            Console.WriteLine($"Password reset mail sent successfully to: {passwordResetMessage.Email}");
        }
    }
}