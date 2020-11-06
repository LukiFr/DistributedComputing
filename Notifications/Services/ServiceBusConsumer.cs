using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class ServiceBusConsumer
    {
        private readonly QueueClient _queueClient;

        private const string QueueName = "messages";

        public ServiceBusConsumer(IConfiguration configuration)

        {

            _queueClient = new QueueClient(

            configuration.GetConnectionString("ServiceBusConnectionString"),

            QueueName);

        }

        public void Register()
        {
            var options = new MessageHandlerOptions((e) => Task.CompletedTask)
            {
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessage, options);
        }

        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            var payload = JsonConvert.DeserializeObject<MessagePayload>(Encoding.UTF8.GetString(message.Body));
            Console.WriteLine("Przetwarzam wiadomosc");

            if(payload.EventName == "NewUserRegistered")
            {
                EmailSender sender = new EmailSender();
                sender.SendNewUserEmail(payload.EmailAddress);
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }

    public class MessagePayload
    {
        public string EventName { get; set; }

        public string EmailAddress { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

    }
}
