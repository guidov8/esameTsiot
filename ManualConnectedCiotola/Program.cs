using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;

namespace ManualConnectedCiotola
{
    class Program
    {
        public int numeroDiDosi { get; set; }

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
                mandaMessaggio(client);
                Task.Delay(3000).Wait();
            }
        }


        static void mandaMessaggio(DeviceClient client)
        {
            string messaggio = "aggiungiDose";
            Byte[] bytes = Encoding.UTF8.GetBytes(messaggio);
            Message message = new Message(bytes);
            client.SendEventAsync(message);
            Console.WriteLine(messaggio);
        }
    }
}
