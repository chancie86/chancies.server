﻿using System.Linq;
using Autofac;
using chancies.Server.Persistence.Cosmos.Startup;

namespace chancies.Server.Persistence.Cosmos
{
    public class PersistenceModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var singletons = new[]
            {
                typeof(CosmosService),
                typeof(Initialiser)
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
