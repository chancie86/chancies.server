using chancies.Server.Persistence.Cosmos.Interfaces;
using chancies.Server.Persistence.Models;
using chancies.Server.Persistence.Repositories;

namespace chancies.Server.Persistence.Cosmos.Implementations
{
    public class SectionRepository
        : BaseRepository<Section, SectionId, SectionListItem>, ISectionRepository
    {
        public SectionRepository(ICosmosService cosmosService)
            : base(cosmosService)
        {
        }
    }
}
