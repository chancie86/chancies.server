using System.Net;
using System.Text;
using System.Threading.Tasks;
using chancies.Server.Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using chancies.Server.Auth.Interfaces;

namespace chancies.Server.Api.FunctionApp.Functions.test
{
    public class TestFunction
    {
        private readonly IAuthenticator _authenticator;

        public TestFunction(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }
        
        [FunctionName("Test")]
        public async Task<ActionResult<string>> Test(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "test")]
            HttpRequest req)
        {
            try
            {
                var (user, token) = await _authenticator.AuthenticateAsync(req, Permissions.Document.Create);

                var sb = new StringBuilder();
                foreach (var claim in user.Claims)
                {
                    sb.AppendLine($"{claim.Type}: {claim.Value}");
                }

                return sb.ToString();
            }
            catch (UnauthorizedException)
            {
                return new UnauthorizedResult();
            }
            catch (ForbiddenException)
            {
                return new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}
