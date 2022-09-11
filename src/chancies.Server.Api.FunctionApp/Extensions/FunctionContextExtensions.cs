using System;
using System.Reflection;
using Microsoft.Azure.Functions.Worker;

namespace chancies.Server.Api.FunctionApp.Extensions
{
    internal static class FunctionContextExtensions
    {
        public static T GetAttribute<T>(this FunctionContext self)
        {
            var functionEntryPoint = self.FunctionDefinition.EntryPoint;
            var assemblyType = Type.GetType(functionEntryPoint.Substring(0, functionEntryPoint.LastIndexOf('.')));
            var methodInfo =
                assemblyType.GetMethod(functionEntryPoint.Substring(functionEntryPoint.LastIndexOf('.') + 1));

            if (methodInfo.GetCustomAttribute(typeof(T), false) is T
                attribute)
            {
                return attribute;
            }

            return default;
        }
    }
}
