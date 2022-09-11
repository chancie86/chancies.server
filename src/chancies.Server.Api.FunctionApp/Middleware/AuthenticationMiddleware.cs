using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using chancies.Server.Api.FunctionApp.Extensions;
using chancies.Server.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace chancies.Server.Api.FunctionApp.Middleware
{
    internal class AuthenticationMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly IAuthenticator _authenticator;

        public AuthenticationMiddleware(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var requestData = await context.GetHttpRequestDataAsync();

            if (requestData == null)
            {
                throw new AuthenticationException("Authorization header missing");
            }

            var authAttribute = context.GetAttribute<AuthorizeAttribute>();
            if (authAttribute != null)
            {
                var authorizationValue = requestData.Headers.GetValues("Authorization").Single();
                _ = await _authenticator.AuthenticateAsync(authorizationValue, authAttribute.Policy);
            }
            
            await next(context);
        }
    }
}
