using System.Threading.Tasks;
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
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder builder) =>
                {
                    builder.UseMiddleware<AuthenticationMiddleware>();
                })
                .ConfigureServices(services =>
                {
                    // Register configuration
                    services
                        .AddConfigurationOptions<AzureConfig>("Azure")
                        .AddConfigurationOptions<Auth0Config>("Auth0");

                    services.AddFunctionServices();
                })
                .Build();

            var cosmosService = host.Services.GetRequiredService<ICosmosService>();
            await cosmosService.Initialize();

            await host.RunAsync();
        }
    }
}