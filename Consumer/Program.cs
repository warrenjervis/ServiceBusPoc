using System.Text;
using Azure.Messaging.ServiceBus;

Console.WriteLine("Starting consumer...");

const string connectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "queue.1";

var client = new ServiceBusClient(connectionString);

var receiverOptions = new ServiceBusReceiverOptions
{
    ReceiveMode = ServiceBusReceiveMode.PeekLock
};

var receiver = client.CreateReceiver(queueName, receiverOptions);

while (true)
{
    var message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
    if (message == null)
    {
        await Task.Delay(1_000);
        continue;
    }
    
    var messageText = Encoding.UTF8.GetString(message.Body);
    Console.WriteLine($"Message received: {messageText}");
    await receiver.CompleteMessageAsync(message);
}
