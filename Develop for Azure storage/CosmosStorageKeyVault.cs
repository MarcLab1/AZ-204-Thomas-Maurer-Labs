using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace CosmosStorageKeyVault
{
    class CosmosStorageKeyVault
    {
        private string CosmosEndpointUrl = "https://cosmosthomas000.documents.azure.com:443/";
        private string CosmosPrimaryKey = "32234==";
        private string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=stthomas000;AccountKey=23324==;EndpointSuffix=core.windows.net";

        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        private BlobServiceClient blobServiceClient;

        private string databaseId = "database001";
        private string containerId = "container001";

        private string kvName = "kvthomas000";

        public static Person p1, p2;
        private static Random rando;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("starting program");

            CosmosStorageKeyVault p = new CosmosStorageKeyVault();
            rando = new Random();

            //create cosmos db, then create container, the add items to container
            //this stuff all happends asyc, so use async methods
            //await p.doCosmos();

            //play with storage accounts
            //await p.doStorage();

            //keyvault is also fun
            await p.doKeyVault();
        }

        private async Task doCosmos()
        {
            p1 = new Person();
            p1.id = rando.Next(999999) + "";
            p1.name = "Jin";
            p1.email = "jin@yahoo.com";

            p2 = new Person();
            p2.id = rando.Next(999999) + "";
            p2.name = "Marc";
            p2.email = "marc@yahoo.com";

            this.cosmosClient = new CosmosClient(CosmosEndpointUrl, CosmosPrimaryKey);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/id"); //primary key must be passed as string

            await container.CreateItemAsync(p1, new PartitionKey(p1.id));
            await container.CreateItemAsync(p2, new PartitionKey(p2.id));
        }

        private async Task doStorage()
        {
            blobServiceClient = new BlobServiceClient(storageConnectionString);
            string containerName = "container" + rando.Next(10000);

            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobContainerClient containerClient2 = blobServiceClient.GetBlobContainerClient("containerfake");

            // List all blobs in the container
            await foreach (BlobItem blobItem in containerClient2.GetBlobsAsync())
            {
                Console.WriteLine(blobItem.Name + "\n");
            }
        }

        private async Task doKeyVault()
        {
            string kvUri = "https://" + kvName + ".vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var secret = new KeyVaultSecret("password3", "1234");
            await client.SetSecretAsync(secret);
            var pass = await client.GetSecretAsync("password3");
            Console.WriteLine("password is "+ pass.Value.Value);
        }
    }
}