using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

const string connectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "myqueue";

await using var client = new ServiceBusClient(connectionString);
await using ServiceBusSender sender = client.CreateSender(queueName);

try
{
    string messageBody = "Hello ASB!";
    var message = new ServiceBusMessage(messageBody);
    await sender.SendMessageAsync(message);
    Console.WriteLine($"Message sent: {messageBody}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error sending message: {ex.Message}");
}
