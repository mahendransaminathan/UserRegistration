using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net;

namespace User.Function
{
    public class UserRegistration
    {
        private readonly ILogger<UserRegistration> _logger;

        private readonly IUserService _userService;

        public UserRegistration(ILogger<UserRegistration> logger, IUserService userService)
        {
            _userService = userService;
        
            _logger = logger;
        }

        [Function("UserRegistration")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            // Parse the request body to get the user details
            if (req.Method == "GET")
            {
                
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var email = query["email"];

                if (string.IsNullOrEmpty(email))
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Email is required.");
                    return badResponse;
                }

                var user = _userService.GetUserByEmail(email);
                if (user == null)
                {
                    var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                    await notFoundResponse.WriteStringAsync("User not found.");
                    return notFoundResponse;
                }

                var okResponse = req.CreateResponse(HttpStatusCode.OK);
                await okResponse.WriteAsJsonAsync(user);
                return okResponse;
                
            }
            else if (req.Method == "POST")
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var user = JsonSerializer.Deserialize<UserReg>(requestBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid user data.");
                    return badResponse;
                }                

                _userService.AddUser(user);
                var createdResponse = req.CreateResponse(HttpStatusCode.Created);
                await createdResponse.WriteAsJsonAsync(user);
                return createdResponse;
            }
            var defaultResponse = req.CreateResponse(HttpStatusCode.OK);
            await defaultResponse.WriteStringAsync("Welcome to Azure Functions!");
            return defaultResponse;
        }
    }
}
