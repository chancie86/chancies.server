using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chancies.Server.Common.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddConfigurationOptions<T>(this IServiceCollection self, string sectionName)
            where T : class
        {
            self
                .AddOptions<T>()
                .Configure<IConfiguration>((settings, configuration)
                    => configuration.GetSection(sectionName).Bind(settings));
            return self;
        }
    }
}
