using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace FunctionAppManagedIdentity
{
    public static class FunctionKeyVaultManagedIdentity
    {
        private static string kvName = "kvthomas000";
        private static string secretName = "secret1";
        private static string kvUri = "https://" + kvName + ".vault.azure.net";

        [FunctionName("FunctionKeyVaultManagedIdentity")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var secret = client.GetSecret(secretName);
            return new OkObjectResult(secret.Value.Value.ToString());
        }
    }
}