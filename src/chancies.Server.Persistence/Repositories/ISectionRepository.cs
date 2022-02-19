using chancies.Server.Persistence.Models;
using chancies.server.Persistence.Repositories;

namespace chancies.Server.Persistence.Repositories
{
    public interface ISectionRepository
        : ICrudRepository<Section, SectionId, SectionListItem>
    {
    }
}
