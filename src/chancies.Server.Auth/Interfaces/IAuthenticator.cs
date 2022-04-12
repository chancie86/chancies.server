using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace chancies.Server.Auth.Interfaces
{
    public interface IAuthenticator
    {
        Task<(ClaimsPrincipal User, SecurityToken ValidatedToken)> AuthenticateAsync(
            HttpRequest request,
            params string[] permissions);
    }
}
