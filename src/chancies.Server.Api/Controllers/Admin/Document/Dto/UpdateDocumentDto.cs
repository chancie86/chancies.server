using System;
using System.Collections.Generic;
using chancies.Server.Persistence.Models;

namespace chancies.Server.Api.Controllers.Admin.Document.Dto
{
    public class UpdateDocumentDto
    {
        public string Name { get; set; }
        public IList<DocumentElement> Elements { get; set; }
        public Guid SectionId { get; set; }
    }
}
