using chancies.Server.Blog.Extensions;
using chancies.Server.Persistence.Cosmos.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace chancies.Server.Api.FunctionApp.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFunctionServices(this IServiceCollection self)
        {
            return self
                .AddCosmosPersistence()
                .AddBlogPersistence();
        }
    }
}
