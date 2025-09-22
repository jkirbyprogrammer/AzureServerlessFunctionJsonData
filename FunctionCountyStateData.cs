using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


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

        _logger.LogInformation("C# HTTP trigger function processed get county data.");
        string type = req.Query["type"].ToString();
        string year = req.Query["year"].ToString();
        FileNameHelper fileNameHelper = new(type, year);
        string fileName = fileNameHelper.GetCountyFileName();
        string connectionString = _configuration.GetConnectionString("BlobConnectionString") ?? ""; //connection string from local.settings.json for Azure Blob storage
        AzureBlobHelper azureBlobHelper = new(connectionString, fileName);
        string jsonString = await azureBlobHelper.GetBlobContentAsync(fileName);

        return new OkObjectResult(jsonString);
    }

    [Function("FunctionGetStateData")]
    public async Task<IActionResult> FunctionGetStateData([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {

        _logger.LogInformation("C# HTTP trigger function processed get state data.");
        string type = req.Query["type"].ToString();
        string year = req.Query["year"].ToString();
        FileNameHelper fileNameHelper = new(type, year);
        string fileName = fileNameHelper.GetStateFileName();
        string connectionString = _configuration.GetConnectionString("BlobConnectionString") ?? ""; //connection string from local.settings.json for Azure Blob storage.
        AzureBlobHelper azureBlobHelper = new(connectionString, fileName);
        string jsonString = await azureBlobHelper.GetBlobContentAsync(fileName);

        return new OkObjectResult(jsonString);
    }

    [Function("FunctionGetFirePointData")]
    public async Task<IActionResult> FunctionGetFirePointData([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {

        _logger.LogInformation("C# HTTP trigger function processed get fire point data.");
        string year = req.Query["year"].ToString();
        FileNameHelper fileNameHelper = new("", year);
        string fileName = fileNameHelper.GetFireFileName();
        string connectionString = _configuration.GetConnectionString("BlobConnectionString") ?? ""; //connection string from local.settings.json for Azure Blob storage
        AzureBlobHelper azureBlobHelper = new(connectionString, fileName);
        string jsonString = await azureBlobHelper.GetBlobContentAsync(fileName);

        return new OkObjectResult(jsonString);
    }


}