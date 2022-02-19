using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace chancies.Server.Persistence.Cosmos.Interfaces
{
    public interface ICosmosService
    {
        Task Initialize();

        Container GetContainer();
    }
}
