using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text;

namespace AzureFunctionJsonData
{
    internal class AzureBlobHelper
    {
        private readonly string _connectionString;
        private readonly string _containerName = "mapjsonfiles"; //Name of your blob container here.
        private readonly string _fileName;

        public AzureBlobHelper(string connectionString, string filename)
        {
            _connectionString = connectionString;
            _fileName = filename;
        }

        public async Task<string> GetBlobContentAsync(string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(_fileName);
            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

            string jsonString = Encoding.UTF8.GetString(downloadResult.Content.ToArray());

            return jsonString;
        }
    }
}
