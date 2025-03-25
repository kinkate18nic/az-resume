using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using System.Data.Common;
using System.Configuration;
using System.Net.Http;
using Microsoft.Azure.Cosmos;
using System.Text;

namespace Company.Function
{
    public static class AzureResumeCounter
    {
        [FunctionName("AzureResumeCounter")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"AzureResume", containerName: "Counter", Connection = "AzureResumeConnectionString", Id="1", PartitionKey="1")] Counter counter,
            [CosmosDB(databaseName:"AzureResume", containerName: "Counter", Connection = "AzureResumeConnectionString", Id="1", PartitionKey="1")] out Counter updatedCounter,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            updatedCounter = counter;
            updatedCounter.Count += 1;

            var jsonReturn = JsonConvert.SerializeObject(counter);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonReturn, Encoding.UTF8, "application/json")
            };
        }

        [FunctionName("GetFunctionApiUrl")]
        public static IActionResult GetFunctionApiUrl(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetFunctionApiUrl")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Fetching Function API URL securely.");

            // Retrieve the function key from environment variables
            string functionKey = Environment.GetEnvironmentVariable("AZUREFUNCTIONKEY");

            if (string.IsNullOrEmpty(functionKey))
            {
                log.LogError("Function Key not found.");
                return new BadRequestObjectResult("Function Key not found.");
            }

            // Construct the full API URL
            string functionApiUrl = $"https://getresumecounternish.azurewebsites.net/api/AzureResumeCounter?code={functionKey}";

            return new OkObjectResult(new { url = functionApiUrl });
        }
    }
}
