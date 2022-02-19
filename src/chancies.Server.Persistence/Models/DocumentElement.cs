using System;
using chancies.Server.Persistence.Converters;
using Newtonsoft.Json;

namespace chancies.Server.Persistence.Models
{
    [JsonConverter(typeof(DocumentElementConverter))]
    public class DocumentElement
    {
        public DocumentElement()
        {
        }

        public virtual DocumentElementType Type => throw new NotImplementedException();

        public Guid Id { get; set; }
    }
}
