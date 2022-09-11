using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using chancies.Server.Api.FunctionApp.Permissions;
using chancies.Server.Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using chancies.Server.Auth.Interfaces;

namespace chancies.Server.Api.FunctionApp.Functions
{
    public class BaseFunction
    {
        private readonly IAuthenticator _authenticator;

        public BaseFunction(
            IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }
        
        protected async Task<ActionResult<TResult>> InvokeAuthenticatedAsync<TResult>(
            HttpRequest req,
            Func<Task<TResult>> func,
            params BaseClaim[] claims)
        {
            try
            {
                var (user, token) = await _authenticator.AuthenticateAsync(req, claims.Select(x => x.ToString()).ToArray());
                return await func();
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
