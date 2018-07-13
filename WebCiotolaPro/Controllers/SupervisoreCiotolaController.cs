using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;
using Message = Microsoft.Azure.Devices.Client.Message;

namespace WebCiotolaPro.Controllers
{
    public class SupervisoreCiotolaController : Controller
    {
        private IConfiguration _configuration;

        public SupervisoreCiotolaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<JsonResult> GetImmagine()
        {/*
            var storageAccount = CloudStorageAccount.Parse(_configuration["StorageConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var photoContainer = blobClient.GetContainerReference("foto");
            var blobs = await photoContainer.ListBlobsSegmentedAsync(null, true, BlobListingDetails.All, 100, null, null, null);


            foreach (var blob in blobs.Results)
            {
                if (blob is CloudBlockBlob)
                {
                    var blockBlob = (CloudBlockBlob)blob;

                    var url = blockBlob.StorageUri.PrimaryUri.ToString();

                    var date = Convert.ToDateTime(blockBlob.Properties.LastModified.Value.DateTime).ToString();

                    list.Add(url);
                    list.Add(date);
                }
            }
            */
            var list = new List<string>();
            return new JsonResult(list);
            
        }



        public async Task<JsonResult> AggiugiDose(string messaggio)
        {

            var rm = RegistryManager.CreateFromConnectionString(_configuration["IotHubConnectionString"]);

            var deviceId = _configuration["deviceId"];

            var deviceKey = _configuration["deviceKey"];

            var transportType = TransportType.Mqtt_WebSocket_Only;

            var autenticationMethod = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);

            var client = DeviceClient.Create(_configuration["hostName"], autenticationMethod, transportType);

            Byte[] bytes = Encoding.UTF8.GetBytes(messaggio);

            Message message = new Message(bytes);

            await client.SendEventAsync(message);

            return new JsonResult("andata");
        }
    }
}