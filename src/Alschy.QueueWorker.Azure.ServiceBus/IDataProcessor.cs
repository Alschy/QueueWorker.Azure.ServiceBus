using Azure.Messaging.ServiceBus;

namespace Alschy.QueueWorker.Azure.ServiceBus;
public interface IDataProcessor
{
    Task ProcessMessageAsync(ProcessMessageEventArgs message);

    Task ProcessErrorAsync(ProcessErrorEventArgs error);
}
