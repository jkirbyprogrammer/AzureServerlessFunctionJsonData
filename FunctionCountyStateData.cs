using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AzureFunctionJsonData;

public class FunctionCountyStateData
{
    private readonly ILogger<FunctionCountyStateData> _logger;
    private readonly IConfiguration _configuration;

    public FunctionCountyStateData(ILogger<FunctionCountyStateData> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [Function("FunctionGetCountyData")]
    public async Task<IActionResult> FunctionGetCountyData([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        string type = req.Query["type"].ToString();
        string year = req.Query["year"].ToString();
        FileNameHelper fileNameHelper = new(type, year);
        string fileName = fileNameHelper.GetCountyFileName();

        string connectionString = _configuration.GetConnectionString("BlobConnectionString") ?? ""; //connection string from local.settings.json for Azure Blob storage
        string containerName = "mapjsonfiles"; //Container name
        string jsonFileName = fileName; // File name
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(jsonFileName);
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        string jsonString = Encoding.UTF8.GetString(downloadResult.Content.ToArray());

        return new OkObjectResult(jsonString);
    }

    [Function("FunctionGetStateData")]
    public async Task<IActionResult> FunctionGetStateData([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        string type = req.Query["type"].ToString();
        string year = req.Query["year"].ToString();
        FileNameHelper fileNameHelper = new(type, year);
        string fileName = fileNameHelper.GetStateFileName();

        string connectionString = _configuration.GetConnectionString("BlobConnectionString") ?? ""; //connection string from local.settings.json for Azure Blob storage
        string containerName = "mapjsonfiles"; //Container name
        string jsonFileName = fileName; //"2025StateUsSecLayer.json"; // File name
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(jsonFileName);
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

        string jsonString = Encoding.UTF8.GetString(downloadResult.Content.ToArray());

        return new OkObjectResult(jsonString);
    }


}