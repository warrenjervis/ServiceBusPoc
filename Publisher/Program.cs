using System.Text;
using Azure.Messaging.ServiceBus;

Console.WriteLine("Starting publisher...");

const string connectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "topic.1";

await using (var client = new ServiceBusClient(connectionString))
{
    var sender = client.CreateSender(topicName);

    // The first 50 messages will go to Subscription 1 and Subscription 3 as per set filters in Config.json
    for (var i = 1; i <= 50; i++)
    {
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes($"Message number : {i}"))
        {
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message);
    }

    // The next 50 messages will go to Subscription 2 and Subscription 3 as per set filters in Config.json

    for (var i = 51; i <= 100; i++)
    {
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes($"Message number : {i}"));
        message.ApplicationProperties.Add("prop1", "value1");

        await sender.SendMessageAsync(message);
    }

    // The next 50 messages will go to Subscription 3 and Subscription 4 as per set filters in Config.json

    for (var i = 101; i <= 150; i++)
    {
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes($"Message number : {i}"))
        {
            MessageId = "123456"
        };
        message.ApplicationProperties.Add("userProp1", "value1");

        await sender.SendMessageAsync(message);
    }
}

Console.WriteLine("Messages published to topic. Exiting publisher.");