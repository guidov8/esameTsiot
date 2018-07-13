using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;


namespace testReceiver
{
    public class IoTHubProcessor : IEventProcessor
    {

        private CloudStorageAccount _account;
        private CloudBlobClient _client;

        public int DosiDisponibili = 30;

        public IoTHubProcessor(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudBlobClient();
        }

        Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.CompletedTask;
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Started partition {context.PartitionId}");
            return Task.CompletedTask;
        }

        Task IEventProcessor.ProcessErrorAsync(PartitionContext context, Exception error)
        {
            return Task.CompletedTask;
        }

        Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                string data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                Console.WriteLine($"Message received: '{data}'");

                if (data == "aggiungiDoseManualCiotola")
                {
                    DosiDisponibili--;
                    Console.WriteLine("Aggiunta una dose\n le dosi disponibili sono:" + DosiDisponibili);
                }

                if( data == "scattaFotoManualCiotola")
                {
                    /*
                    var rm = RegistryManager.CreateFromConnectionString(_configuration["IotHubConnectionString"]);

                    var deviceId = _configuration["deviceId"];

                    var deviceKey = _configuration["deviceKey"];

                    var transportType = TransportType.Mqtt_WebSocket_Only;

                    var autenticationMethod = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);

                    var client = DeviceClient.Create(_configuration["hostName"], autenticationMethod, transportType);

                    Byte[] bytes = Encoding.UTF8.GetBytes(messaggio);

                    Message message = new Message(bytes);

                    await client.SendEventAsync(message);

                    */
                }
            }

            context.CheckpointAsync().Wait();
            return Task.CompletedTask;
        }
    }
}
