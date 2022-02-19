using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace chancies.Server.Persistence.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DocumentElementType
    {
        Html,
        Images,
        Video
    }
}
