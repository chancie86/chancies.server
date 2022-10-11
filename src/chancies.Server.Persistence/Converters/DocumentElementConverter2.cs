using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using chancies.Server.Persistence.Models;

namespace chancies.Server.Persistence.Converters
{
    public class DocumentElementConverter2
        : JsonConverter<DocumentElement>
    {
        public override DocumentElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, DocumentElement value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteString(nameof(DocumentElement.Type), value.Type.ToString());
            writer.WriteString(nameof(DocumentElement.Id), value.Id);

            switch (value)
            {
                case HtmlDocumentElement hde:
                    writer.WriteString(nameof(HtmlDocumentElement.Content), hde.Content);
                    break;
                case ImagesDocumentElement ide:
                    writer.WriteStartArray(nameof(ImagesDocumentElement.Images));

                    foreach (var image in ide.Images)
                    {
                        writer.WriteStartObject();
                        writer.WriteString(nameof(DocumentImage.Path), image.Path);
                        writer.WriteString(nameof(DocumentImage.Title), image.Title);
                        writer.WriteEndObject();
                    }
                    
                    writer.WriteEndArray();
                    break;
                case VideoDocumentElement vde:
                    writer.WriteString(nameof(VideoDocumentElement.Url), vde.Url);
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
