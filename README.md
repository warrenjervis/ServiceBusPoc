# Azure Service Bus Proof of Concept

This POC consists of four simple console apps, meant to be ran as pairs:
- Producer/Consumer - these apps simulate a simple queue or messaging system, where one service sends messages to another service.
- Publisher/Subscriber - these apps simulate topics and subscriptions, a one-to-many form of communication, where a publisher may send messages to many subscribers.

## Setup

### Azure Service Bus Local Emulator

Under the service folder, is a Docker Compose file, that will create and run a local emulator. Copy the `.env.sample` file, to a new `.env` file. Replace the values as needed, then run `docker compose up -d`, to start the service.


### Producer/Consumer

Build the projects, and run the producer. It'll prompt for messages to send. Enter as many messages as you'd like. Run the consumer, and it'll output the messages received. If left running, you can continue to send messages via the Producer, and see them as they appear in the Consumer.

### Publisher/Subscriber

Build the projects, and run the Publisher. It'll publish 150 messages, to a topic, which is defined in the `./service/Config.json` file. The messages will be distributed to four different subscriptions, based on the message properties. Run the Subscriber, and it'll prompt which of the four subscriptions you want to receive messages for. Enter the subscription, or specify all of them, and the messages sent to the subscription will appear.
