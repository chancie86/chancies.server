﻿namespace chancies.Server.Persistence.Models
{
    public class DocumentListItem
        : BaseDataModel<DocumentId>
    {
        public SectionId SectionId { get; set; }
        public bool Published { get; set; }
    }
}
