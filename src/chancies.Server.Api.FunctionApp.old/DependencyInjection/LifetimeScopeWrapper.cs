using System;
using Autofac;

namespace chancies.Server.Api.FunctionApp.DependencyInjection
{
    internal sealed class LifetimeScopeWrapper : IDisposable
    {
        public ILifetimeScope Scope { get; }

        public LifetimeScopeWrapper(IContainer container)
        {
            Scope = container.BeginLifetimeScope();
        }

        public void Dispose()
        {
            Scope.Dispose();
        }
    }

}
