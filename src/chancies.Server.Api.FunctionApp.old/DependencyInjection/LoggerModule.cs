﻿using Autofac;
using Microsoft.Extensions.Logging;

namespace chancies.Server.Api.FunctionApp.DependencyInjection
{
    internal class LoggerModule : Module
    {
        public const string LoggerFactoryParam = "loggerFactory";
        public const string FunctionNameParam = "functionName";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((ctx, p) => p.Named<ILoggerFactory>(LoggerFactoryParam))
                .SingleInstance();

            builder.Register((ctx, p) =>
                {
                    var factory = ctx.Resolve<ILoggerFactory>();
                    var functionName = p.Named<string>(FunctionNameParam);

                    return factory.CreateLogger(Microsoft.Azure.WebJobs.Logging.LogCategories.CreateFunctionUserCategory(functionName));
                })
                .InstancePerLifetimeScope();
        }
    }

}
