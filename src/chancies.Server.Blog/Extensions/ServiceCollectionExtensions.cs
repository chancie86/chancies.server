using chancies.Server.Blog.Implementation;
using chancies.Server.Blog.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace chancies.Server.Blog.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlogPersistence(this IServiceCollection self)
        {
            return self
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<ISectionService, SectionService>();
        }
    }
}
