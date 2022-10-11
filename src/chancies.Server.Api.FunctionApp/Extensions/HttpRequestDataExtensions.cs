using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace chancies.Server.Api.FunctionApp.Extensions
{
    internal static class HttpRequestDataExtensions
    {
        public static T ReadBody<T>(this HttpRequestData self)
        {
            return JsonSerializer.Deserialize<T>(self.Body);
        }
    }
}
