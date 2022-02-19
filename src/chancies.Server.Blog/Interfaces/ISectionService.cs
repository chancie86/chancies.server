using System.Collections.Generic;
using System.Threading.Tasks;
using chancies.Server.Persistence.Models;

namespace chancies.Server.Blog.Interfaces
{
    public interface ISectionService
    {
        Task<SectionId> Create(Section section);
        Task<Section> Get(SectionId id);
        Task<IList<SectionListItem>> List();
        Task Delete(SectionId id);
        Task Update(Section section);
    }
}
