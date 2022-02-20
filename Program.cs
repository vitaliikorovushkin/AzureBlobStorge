using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStorage
{
    class Program
    {
         static async Task Main()
         {
             var configuration = new ConfigurationBuilder()
                .AddJsonFile("jsconfig.json")
                .Build();

            //create source directory
            string localPath = "./data/";
            // If directory does not exist, create it
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string fileName = "analemma1200.png";
            string localFilePath = Path.Combine(localPath, fileName);

            string connectionString = configuration["StorageConnectionString"];
            //CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            //await CreateContainer(connectionString, "fromvs1");
            //await DisplayContainerContent(connectionString, "fromvs1");
            //await AddToContainer(connectionString, "fromvs1", localFilePath);
            await GetFromContainer(connectionString, "fromvs1", "analemma1200.png");
        }
        static async Task CreateContainer(string connectionString, string containerName)
        {
            //Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        }// создает контейнер на Ажуре

        static async Task DisplayContainerContent(string connectionString, string containerName)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            Console.WriteLine("Listing blobs...");
            // List all blobs in the container
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }
        }// отображает содержимое контейнера в консоль

        static async Task AddToContainer(string connectionString, string containerName, string filePath)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);

            string fileName = Path.GetFileName(filePath);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
            await blobClient.UploadAsync(filePath, true);
        }// добавляет файл в контейнер

        static async Task GetFromContainer(string connectionString, string containerName, string fileName)
        {
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            string localPath = "./data/";
            string localFilePath = Path.Combine(localPath, fileName);
            string downloadFilePath = localFilePath.Replace(".png", "_DOWNLOADED.png");

            Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            // Download the blob's contents and save it to a file
            await blobClient.DownloadToAsync(downloadFilePath);
        }// получает файл из контейнера

    }
}
