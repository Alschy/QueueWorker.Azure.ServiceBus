# QueueWorker.Azure.ServiceBus
A simple, tiny and specialized library for creating microservices they should consume Azure Service Bus messages.

## For who is the library written?
For all they would implement in a stright forward manner, microservice they should listen on Azure Service Bus Messages and would easy implement the business logic than archtitectural and technical boilerplate code.

## For who is the library NOT wirtten
- For all they would use other message brokers than Azure Service Bus. Sorry, it would not be happen in this library.
- For all they love her own architecture and wanna have full control over the archtecture and the message processing.
- For all overengineers.

## STS approach
- S -> Simple. No complicated options and boilerplate code requiered to use it (it should reduce the code that you must be write)
- T -> Tiny. No much classes, no much options, a very limited use case. Thats the recipe for a library that is fast, small, maintainable and understoodable. It's contain only three classes including the one extension class for the Microsoft DI system.
- S -> Specilized. Specialized for a limited set of use cases, in practise I use in one project one technology for multiple business cases, they could implemented by a single technology. There are a lot reasons to support multiple message brokers, like in a product. But there a allways the customer individual projects, where you have the possibility to choose only one message broker.

## How to use it?
NICE, that you ask ;)

1. Install the nuget package.
2. Add a class in which you would process your messages and implement the interface `IDataProcessor` implement the two methods from the interface:
  - `ProcessMessageAsync` returns a `Task` for asyncronusly operations. Became a `ProcessMessageEventArgs` parameter which is representing the Azure Service Bus message.
  - `ProcessErrorAsync` returns a `Task` too, for same reasons. Became a `ProcessErrorEventArgs` parameter who containes the error of the message processing. would be generated from the Azure Service Bus client library.
3. Go to your `Programm.cs` and build your hosting system (for now only the Microsoft DI library is direct supported, but technically you can use othe IoC libraries, i think).
3.1. Add to a configured `ServiceBusClient` to the IoC. For simple reasons you can use the `AddServiceBusClient(string? connectionString)` method if you want. If you wanna to managed identiies or othe authentication methods then you can easily do your stuff in your code.
3.1. Now the interssting part :) Call `AddDataQueueProcessor<TProcessor>(string queueName)` `TProcessor` must be your data processor you have implemented in step 2. What would be do this? First its register your data processor to the IoC, then a hosted service would be created. To build th hosted service, its receive the `ServiceBusClient` from the IoC, get from this a queue processor with your desired queue name and then create a new `QueueReceiverJob` with the type of your processor.
4. Lean back and implement your business logic.
