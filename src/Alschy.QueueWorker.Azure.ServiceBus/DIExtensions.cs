using Alschy.QueueWorker.Azure.ServiceBus;
using Azure.Messaging.ServiceBus;

namespace Microsoft.Extensions.DependencyInjection;
public static class DIExtensions
{
    public static IServiceCollection AddServiceBusClient(this IServiceCollection services, string? connectionString, ServiceBusClientOptions? clientOptions = default)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "The connectionString should not be null or empty.");
        }

        clientOptions ??= new ServiceBusClientOptions();

        return services.AddScoped(sp => new ServiceBusClient(connectionString, clientOptions));
    }

    public static IServiceCollection AddDataQueueProcessor<TProcessor>(this IServiceCollection services, string? queueName)
        where TProcessor : class, IDataProcessor
    {
        if (string.IsNullOrEmpty(queueName))
        {
            throw new ArgumentNullException(nameof(queueName), "The name of the queue, where the processor should listing could not empty or null.");
        }
        services.AddScoped<TProcessor>();

        services.AddHostedService(sp =>
        {
            var serviceBusClient = sp.GetRequiredService<ServiceBusClient>();
            var queueProcessor = serviceBusClient.CreateProcessor(queueName);
            return new QueueReceiverJob<TProcessor>(queueProcessor, sp.GetRequiredService<TProcessor>());
        });
        return services;
    }
}
