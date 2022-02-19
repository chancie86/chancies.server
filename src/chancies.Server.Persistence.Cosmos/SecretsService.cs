using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using chancies.Server.Persistence.Cosmos.Config;
using chancies.Server.Persistence.Cosmos.Interfaces;
using Microsoft.Extensions.Options;

namespace chancies.Server.Persistence.Cosmos
{
    public class SecretsService
        : ISecretsService
    {
        private readonly AzureConfig _config;

        public SecretsService(IOptions<AzureConfig> config)
        {
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<string> GetSecret(string name)
        {
            var client = new SecretClient(new Uri(_config.KeyVaultUrl), new DefaultAzureCredential());
            var secret = await client.GetSecretAsync(name);
            return secret.Value.Value;
        }
    }
}
