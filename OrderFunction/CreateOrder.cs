using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace OrderFunction
{
    public static class CreateOrder
    {
        [FunctionName(nameof(CreateOrder))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("CreateOrder HTTP trigger function RUN processed a request.");

            string destination = req.Query["dest"];
            using (var streamReader = new StreamReader(req.Body))
            {
                var requestBody = await streamReader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                destination = destination ?? data?.destination;

                return destination != null 
                    ? (ActionResult)new OkObjectResult($"Order recieved, destination: {destination}")
                    : new BadRequestObjectResult("Please pass a destination on the request body");
            }
        }
    }
}
