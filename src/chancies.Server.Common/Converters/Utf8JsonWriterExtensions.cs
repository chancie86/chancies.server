using System;
using System.Text.Json;

namespace chancies.Server.Common.Converters
{
    public static class Utf8JsonWriterExtensions
    {
        public static void WriteString(this Utf8JsonWriter self, string name, string value, JsonSerializerOptions options)
        {
            if (options.PropertyNamingPolicy == null)
            {
                self.WriteString(name, value);
                return;
            }

            var convertedName = options.PropertyNamingPolicy.ConvertName(name);
            self.WriteString(convertedName, value);
        }

        public static void WriteString(this Utf8JsonWriter self, string name, Guid value,
            JsonSerializerOptions options)
        {
            self.WriteString(name, value.ToString(), options);
        }

        public static void WriteStartArray(this Utf8JsonWriter self, string name, JsonSerializerOptions options)
        {
            if (options.PropertyNamingPolicy == null)
            {
                self.WriteStartArray(name);
                return;
            }

            var convertedName = options.PropertyNamingPolicy.ConvertName(name);
            self.WriteStartArray(convertedName);
        }
    }
}
