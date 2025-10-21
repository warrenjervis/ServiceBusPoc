using System.Text;
using Azure.Messaging.ServiceBus;

Console.WriteLine("Starting publisher...");

const string connectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "topic.1";

Console.WriteLine("Enter the subscription ID you want to receive messages from (1-4, or 'all'):");
var subscriptionId = Console.ReadLine();

if (subscriptionId?.ToLowerInvariant() == "all")
{
    for (var i = 1; i <= 4; i++)
    {
        await ConsumeMessageFromSubscription($"subscription.{i}");
    }
}
else
{
    await ConsumeMessageFromSubscription($"subscription.{subscriptionId}");
}

return;
async Task ConsumeMessageFromSubscription(string subscriptionName)
{
    Console.WriteLine($"Rcv_Sub {subscriptionName} Begin");

    //  Receive on Sub 1
    var client = new ServiceBusClient(connectionString);
    var processorOptions = new ServiceBusProcessorOptions
    {
        ReceiveMode = ServiceBusReceiveMode.PeekLock
    };
    var processor = client.CreateProcessor(topicName, subscriptionName, processorOptions);

    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();

    await Task.Delay(TimeSpan.FromSeconds(5));

    await processor.StopProcessingAsync();
    await processor.DisposeAsync();
    await client.DisposeAsync();
    Console.WriteLine($"Rcv_Sub {subscriptionName} End");
}

Task ErrorHandler(ProcessErrorEventArgs arg)
{
    Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");
    return Task.CompletedTask;   
}

Task MessageHandler(ProcessMessageEventArgs arg)
{
    var body = Encoding.UTF8.GetString(arg.Message.Body);
    Console.WriteLine($"Received message: SequenceNumber:{arg.Message.SequenceNumber} Body:{body} To:{arg.Message.To}");
    return arg.CompleteMessageAsync(arg.Message);  
}
