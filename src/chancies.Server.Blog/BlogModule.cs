using Autofac;

namespace chancies.Server.Blog
{
    public class BlogModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterTypes(ThisAssembly.GetTypes())
                .AsImplementedInterfaces();
        }
    }
}
