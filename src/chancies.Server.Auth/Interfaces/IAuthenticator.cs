using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace chancies.Server.Auth.Interfaces
{
    public interface IAuthenticator
    {
        Task<(ClaimsPrincipal user, SecurityToken validatedToken)> AuthenticateAsync(
            HttpRequest request,
            params string[] permissions);

        Task<(ClaimsPrincipal user, SecurityToken validatedToken)> AuthenticateAsync(
            string authorizationValue,
            params string[] permissions);
    }
}
