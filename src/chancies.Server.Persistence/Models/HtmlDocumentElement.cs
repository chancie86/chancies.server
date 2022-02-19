namespace chancies.Server.Persistence.Models
{
    public class HtmlDocumentElement
        : DocumentElement
    {
        public string Content { get; set; }
        public override DocumentElementType Type => DocumentElementType.Html;
    }
}
