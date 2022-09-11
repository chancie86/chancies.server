using chancies.Server.Persistence.Cosmos.Implementations;
using chancies.Server.Persistence.Cosmos.Interfaces;
using chancies.Server.Persistence.Cosmos.Startup;
using chancies.server.Persistence.Repositories;
using chancies.Server.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace chancies.Server.Persistence.Cosmos.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosPersistence(this IServiceCollection self)
        {
            return self
                .AddSingleton<ICosmosService, CosmosService>()
                .AddSingleton<Initialiser>()
                .AddSingleton<ISecretsService, SecretsService>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IImageRepository, ImageRepository>()
                .AddScoped<ISectionRepository, SectionRepository>();
        }
    }
}
