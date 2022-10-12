using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using chancies.Server.Common.Converters;
using chancies.Server.Persistence.Models;

namespace chancies.Server.Persistence.Converters
{
    public class DocumentElementConverter2
        : JsonConverter<DocumentElement>
    {
        public override DocumentElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var doc = JsonDocument.ParseValue(ref reader);

            var typePropertyName = options.PropertyNamingPolicy == null
                ? nameof(DocumentElement.Type)
                : options.PropertyNamingPolicy.ConvertName(nameof(DocumentElement.Type));

            var typeElement = doc.RootElement.GetProperty(typePropertyName);

            return typeElement.GetString() switch
            {
                nameof(DocumentElementType.Html) => Deserialize<HtmlDocumentElement>(doc, options),
                nameof(DocumentElementType.Images) => Deserialize<ImagesDocumentElement>(doc, options),
                nameof(DocumentElementType.Video) => Deserialize<VideoDocumentElement>(doc, options),
                _ => throw new InvalidOperationException()
            };
        }

        public override void Write(Utf8JsonWriter writer, DocumentElement value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(DocumentElement.Type), value.Type.ToString(), options);
            writer.WriteString(nameof(DocumentElement.Id), value.Id, options);

            switch (value)
            {
                case HtmlDocumentElement hde:
                    writer.WriteString(nameof(HtmlDocumentElement.Content), hde.Content, options);
                    break;
                case ImagesDocumentElement ide:
                    writer.WriteStartArray(nameof(ImagesDocumentElement.Images), options);

                    foreach (var image in ide.Images)
                    {
                        writer.WriteStartObject();
                        writer.WriteString(nameof(DocumentImage.Path), image.Path, options);
                        writer.WriteString(nameof(DocumentImage.Title), image.Title, options);
                        writer.WriteEndObject();
                    }
                    
                    writer.WriteEndArray();
                    break;
                case VideoDocumentElement vde:
                    writer.WriteString(nameof(VideoDocumentElement.Url), vde.Url, options);
                    break;
            }

            writer.WriteEndObject();
        }

        private static DocumentElement Deserialize<T>(JsonDocument doc, JsonSerializerOptions options)
            where T : DocumentElement
        {
            return JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText(), options);
        }
    }
}
