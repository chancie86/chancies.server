using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

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

        public static async Task SetResponse(this FunctionContext self, HttpStatusCode statusCode, object body = null)
        {
            var requestData = await self.GetHttpRequestDataAsync();
            var response = requestData!.CreateResponse(statusCode);

            if (body != null)
            {
                await response.WriteAsJsonAsync(body, response.StatusCode);
            }

            var invocationResult = self.GetInvocationResult();

            var httpOutputBindingFromMultipleOutputBindings = GetHttpOutputBindingFromMultipleOutputBinding(self);
            if (httpOutputBindingFromMultipleOutputBindings is not null)
            {
                httpOutputBindingFromMultipleOutputBindings.Value = response;
            }
            else
            {
                invocationResult.Value = response;
            }
        }

        private static OutputBindingData<HttpResponseData> GetHttpOutputBindingFromMultipleOutputBinding(FunctionContext context)
        {
            // The output binding entry name will be "$return" only when the function return type is HttpResponseData
            var httpOutputBinding = context.GetOutputBindings<HttpResponseData>()
                .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

            return httpOutputBinding;
        }
    }
}
