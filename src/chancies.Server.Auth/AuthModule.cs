using System.Linq;
using Autofac;
using chancies.Server.Auth.Auth0;

namespace chancies.Server.Auth
{
    public class AuthModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var singletons = new[]
            {
                typeof(Auth0Authenticator)
            };

            builder.RegisterTypes(
                    ThisAssembly
                        .GetTypes()
                        .Where(x => !singletons.Contains(x)).ToArray())
                .AsImplementedInterfaces();

            builder.RegisterTypes(singletons).AsImplementedInterfaces().SingleInstance();
        }
    }
}
