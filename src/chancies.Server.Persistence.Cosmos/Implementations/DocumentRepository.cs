using System.Collections.Generic;
using System.Threading.Tasks;
using chancies.Server.Persistence.Cosmos.Interfaces;
using chancies.Server.Persistence.Models;
using chancies.server.Persistence.Repositories;

namespace chancies.Server.Persistence.Cosmos.Implementations
{
    internal class DocumentRepository
        : BaseRepository<Document, DocumentId, DocumentListItem>, IDocumentRepository
    {
        public DocumentRepository(ICosmosService cosmosService)
            : base(cosmosService)
        {
        }

        public override async Task<IList<DocumentListItem>> List()
        {
            return await ListInternal("sectionId", "published");
        }
    }
}
