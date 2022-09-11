using System;
using chancies.Server.Persistence.Converters;

namespace chancies.Server.Persistence.Models
{
    [Newtonsoft.Json.JsonConverter(typeof(DocumentElementConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(DocumentElementConverter2))]
    public class DocumentElement
    {
        public DocumentElement()
        {
        }

        public virtual DocumentElementType Type => throw new NotImplementedException();

        public Guid Id { get; set; }
    }
}
