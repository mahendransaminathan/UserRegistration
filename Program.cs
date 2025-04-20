using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using UserRegistration.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

    builder.Services.AddSingleton<CosmosClient>(serviceProvider => 
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        // var cosmosEndPoint = Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT");
        // var cosmosKey = Environment.GetEnvironmentVariable("COSMOS_DB_KEY");    

        var cosmosEndPoint = Environment.GetEnvironmentVariable("COSMOS_DB_ENDPOINT");
        var cosmosKey = Environment.GetEnvironmentVariable("COSMOS_DB_KEY");    
        if (string.IsNullOrEmpty(cosmosKey))
        {
            throw new ArgumentNullException("COSMOS_DB_KEY", "❌ Cosmos DB Key is missing");
        }
        return new CosmosClient(cosmosEndPoint, cosmosKey);
    }
    );
    builder.Services.AddScoped<IUserRepository, UserCosmosRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    
builder.Build().Run();
