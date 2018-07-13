using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace PadroncinoInTheCloud
{
    public class IoTHubProcessor : IEventProcessor
    {

        private CloudStorageAccount _account;
        private CloudBlobClient _client;

        public int DosiDisponibili = 20;

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

                if (data == "doseMangiataAutomaticCiotola")
                {

                    try
                    {
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = "Server=tcp:temperatureguido.database.windows.net,1433;Initial Catalog=padroncinoDB;Persist Security Info=False;User ID=rabat;Password=Agfa1997;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                        cnn.Open();
                        IDbConnection db = cnn;//DAPPER



                        if (DosiDisponibili > 5)//il gatto sta consumando le dosi per cui scalo le dosi dalla ciotola nel db
                        {
                            DosiDisponibili--;
                            db.Execute("UPDATE [dbo].[ciotole] SET dosi = " + DosiDisponibili + " WHERE id = 1");
                        }
                        else//ricarico le dosi
                        {
                            DosiDisponibili = 20;
                            db.Execute("UPDATE [dbo].[ciotole] SET dosi = " + DosiDisponibili + " WHERE id = 1");
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }



            }

            context.CheckpointAsync().Wait();
            return Task.CompletedTask;
        }
    }
}
