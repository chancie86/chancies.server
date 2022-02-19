using System.Collections.Generic;
using System.Threading.Tasks;
using chancies.Server.Persistence.Models;

namespace chancies.server.Persistence.Repositories
{
    public interface ICrudRepository<TDocument, TId, TList>
        where TDocument : BaseDataModel<TId>
        where TList : BaseDataModel<TId>
    {
        Task<TId> Create(TDocument document);
        Task Delete(TId id);
        Task<IList<TList>> List();
        Task<TDocument> Read(TId id);
        Task Update(TDocument document);
    }
}
