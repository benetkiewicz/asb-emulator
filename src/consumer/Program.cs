using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

const string connectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "myqueue";
TaskCompletionSource<bool> shutdownSource = new TaskCompletionSource<bool>();

await using var client = new ServiceBusClient(connectionString);

ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions
{
    AutoCompleteMessages = false
});

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Started processing messages. Press Ctrl+C to stop.");
    Console.CancelKeyPress += (s, e) =>
    {
        e.Cancel = true;
        shutdownSource.SetResult(true);
        Console.WriteLine("\nShutdown requested. Completing current message...");
    };

    await shutdownSource.Task;
    await processor.StopProcessingAsync();
}
finally
{
    await processor.DisposeAsync();
}

static async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received message: {body}");

    try
    {
        await args.CompleteMessageAsync(args.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing message: {ex.Message}");
        await args.AbandonMessageAsync(args.Message);
    }
}

static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine($"Error source: {args.ErrorSource}");
    Console.WriteLine($"Error message: {args.Exception.Message}");
    return Task.CompletedTask;
}