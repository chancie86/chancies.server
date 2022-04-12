using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using chancies.Server.Api.FunctionApp;
using chancies.Server.Api.FunctionApp.DependencyInjection;
using chancies.Server.Auth;
using chancies.Server.Auth.Config;
using chancies.Server.Blog;
using chancies.Server.Common.Extensions;
using chancies.Server.Persistence.Cosmos;
using chancies.Server.Persistence.Cosmos.Config;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Startup))]

namespace chancies.Server.Api.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add configuration options
            builder.Services
                .AddConfigurationOptions<AzureConfig>("Azure")
                .AddConfigurationOptions<Auth0Config>("Auth0");
            
            var container = GetContainer(builder.Services);

            builder.Services.AddSingleton(container);

            builder.Services.AddLogging(builder => builder.AddConsole());

            // Important: Use AddScoped so our Autofac lifetime scope gets disposed
            // when the function finishes executing
            builder.Services.AddScoped<LifetimeScopeWrapper>();

            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivator), typeof(AutofacJobActivator)));
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivatorEx), typeof(AutofacJobActivator)));
        }

        private static IContainer GetContainer(IServiceCollection serviceCollection)
        {
            var cb = new ContainerBuilder();

            cb.Populate(serviceCollection);
            cb.RegisterModule<LoggerModule>();
            cb.RegisterModule<AuthModule>();

            // This is a convenient way to register all your function classes at once
            cb
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .InNamespaceOf<Startup>();

            cb.RegisterModule<BlogModule>();
            cb.RegisterModule<PersistenceModule>();

            return cb.Build();
        }
    }
}
