using Azure.Messaging.ServiceBus;

Console.WriteLine("Starting producer...");

const string connectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "queue.1";

var @continue = true;

while (@continue)
{

    Console.WriteLine("Enter message to send to queue:");
    var messageText = Console.ReadLine();

    var client = new ServiceBusClient(connectionString);
    var sender = client.CreateSender(queueName);
    var message = new ServiceBusMessage(messageText);
    await sender.SendMessageAsync(message);

    Console.WriteLine("Message sent to queue. Press 'y' to send another message, or anything else to stop.");
    var key = Console.ReadKey();
    @continue = key.KeyChar == 'y';
    Console.WriteLine();
}

Console.WriteLine("Exiting producer.");
