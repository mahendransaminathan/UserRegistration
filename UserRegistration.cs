using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace User.Function
{
    public class UserRegistration
    {
        private readonly ILogger<UserRegistration> _logger;

        public UserRegistration(ILogger<UserRegistration> logger)
        {
            _logger = logger;
        }

        [Function("UserRegistration")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
