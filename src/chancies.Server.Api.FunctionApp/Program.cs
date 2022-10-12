using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using chancies.Server.Api.FunctionApp.Extensions;
using chancies.Server.Api.FunctionApp.Middleware;
using chancies.Server.Auth.Config;
using chancies.Server.Common.Extensions;
using chancies.Server.Persistence.Cosmos.Config;
using chancies.Server.Persistence.Cosmos.Interfaces;
using Microsoft.Azure.Functions.Worker;

namespace chancies.Server.Api.FunctionApp
{
    public class Program
    {
        public static async Task Main()
        {
            var defaultJsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                PropertyNameCaseInsensitive = true
            };

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder builder) =>
                {
                    builder
                        .UseMiddleware<ExceptionHandlerMiddleware>()
                        .UseMiddleware<AuthenticationMiddleware>();
                    
                }, workerOptions =>
                {
                    workerOptions.Serializer = new JsonObjectSerializer(defaultJsonSerializerOptions);
                })
                .ConfigureServices(services =>
                {
                    // Register configuration
                    services
                        .AddConfigurationOptions<AzureConfig>("Azure")
                        .AddConfigurationOptions<Auth0Config>("Auth0");

                    services.AddSingleton(defaultJsonSerializerOptions);
                    services.AddFunctionServices();
                })
                .Build();

            var cosmosService = host.Services.GetRequiredService<ICosmosService>();
            await cosmosService.Initialize();

            await host.RunAsync();
        }
    }
}