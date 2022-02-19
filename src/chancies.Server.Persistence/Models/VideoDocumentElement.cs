namespace chancies.Server.Persistence.Models
{
    public class VideoDocumentElement
        : DocumentElement
    {
        public string Url { get; set; }
        public override DocumentElementType Type => DocumentElementType.Video;
    }
}
