using System.IO;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;

namespace chancies.Server.Api.FunctionApp.Extensions
{
    internal static class HttpRequestDataExtensions
    {
        public static T ReadBody<T>(this HttpRequestData self)
            where T : class
        {
            if (self.Body == Stream.Null)
            {
                return null;
            }

            var options = self.FunctionContext.InstanceServices.GetRequiredService<JsonSerializerOptions>();
            return JsonSerializer.Deserialize<T>(self.Body, options);
        }
    }
}
