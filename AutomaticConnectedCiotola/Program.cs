using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;

namespace AutomaticConnectedCiotola
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            var deviceId = configuration["deviceId"];
            var deviceKey = configuration["deviceKey"];
            var hostname = configuration["hostName"];

            var transportType = TransportType.Mqtt_WebSocket_Only;
            var autenticationMethod = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);
            var client = DeviceClient.Create(hostname, autenticationMethod, transportType);



            while (true)
            {
                SimulatedGatto(client);
                Task.Delay(5000).Wait();
            }
        }


        static void SimulatedGatto(DeviceClient client)
        {
            string messaggio = "doseMangiataAutomaticCiotola";
            Byte[] bytes = Encoding.UTF8.GetBytes(messaggio);
            Message message = new Message(bytes);
            client.SendEventAsync(message);
            Console.WriteLine(messaggio);
        }
    }
}
