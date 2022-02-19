using System.Threading.Tasks;

namespace chancies.Server.Persistence.Cosmos.Interfaces
{
    public interface ISecretsService
    {
        Task<string> GetSecret(string name);
    }
}
