using System;
using System.IO;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace testReceiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var host = new EventProcessorHost(
                configuration["eventHubPath"],
                configuration["eventHubConsumerGroup"],
                configuration["eventHubConnectionString"],
                configuration["StorageConnectionString"],
                configuration["leaseContainerName"]
            );

            var factory = new IotHubProcessorFactory(
                configuration["StorageConnectionString"]);

            await host.RegisterEventProcessorFactoryAsync(factory);

            Console.WriteLine("IotHubEventProcessor is running...");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("ManulaConnectedCiotola");
            Console.WriteLine("");
            Console.WriteLine("");


            Console.ReadLine();
        }
    }
}
