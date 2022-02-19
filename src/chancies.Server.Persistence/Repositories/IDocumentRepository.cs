using chancies.Server.Persistence.Models;

namespace chancies.server.Persistence.Repositories
{
    public interface IDocumentRepository
        : ICrudRepository<Document, DocumentId, DocumentListItem>
    {
    }
}
