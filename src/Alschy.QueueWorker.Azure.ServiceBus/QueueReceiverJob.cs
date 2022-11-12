using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;

namespace Alschy.QueueWorker.Azure.ServiceBus;
public class QueueReceiverJob<TProcessor> : IHostedService
    where TProcessor : class, IDataProcessor
{
    private readonly ServiceBusProcessor serviceBusProcessor;
    protected readonly TProcessor dataProcessor;

    public QueueReceiverJob(ServiceBusProcessor serviceBusProcessor, TProcessor dataProcessor)
    {
        this.serviceBusProcessor = serviceBusProcessor;
        this.dataProcessor = dataProcessor;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        serviceBusProcessor.ProcessMessageAsync += dataProcessor.ProcessMessageAsync;
        serviceBusProcessor.ProcessErrorAsync += dataProcessor.ProcessErrorAsync;

        await serviceBusProcessor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await serviceBusProcessor.StopProcessingAsync(cancellationToken);
        await serviceBusProcessor.DisposeAsync();
    }
}
