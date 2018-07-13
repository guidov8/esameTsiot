using Microsoft.Azure.EventHubs.Processor;

namespace testReceiver
{
    public class IotHubProcessorFactory : IEventProcessorFactory
    {
        private string _connectionString;
        public IotHubProcessorFactory(string connectionString) => (_connectionString) = (connectionString);
        IEventProcessor IEventProcessorFactory.CreateEventProcessor(PartitionContext context)
        {
            return new IoTHubProcessor(_connectionString);
        }
    }
}
