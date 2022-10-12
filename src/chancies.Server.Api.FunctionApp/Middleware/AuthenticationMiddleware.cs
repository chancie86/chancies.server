using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using chancies.Server.Api.FunctionApp.Extensions;
using chancies.Server.Auth.Exceptions;
using chancies.Server.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace chancies.Server.Api.FunctionApp.Middleware
{
    internal class AuthenticationMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly IAuthenticator _authenticator;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(
            IAuthenticator authenticator,
            ILogger<AuthenticationMiddleware> logger)
        {
            _authenticator = authenticator;
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                var requestData = await context.GetHttpRequestDataAsync();

                if (requestData == null)
                {
                    throw new UnauthorizedException("Authorization header missing");
                }

                var authAttribute = context.GetAttribute<AuthorizeAttribute>();
                if (authAttribute != null)
                {
                    try
                    {
                        var authorizationValue = requestData.Headers.GetValues("Authorization").Single();
                        _ = await _authenticator.AuthenticateAsync(authorizationValue, authAttribute.Policy);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new UnauthorizedException();
                    }
                }

                await next(context);
            }
            catch (UnauthorizedException ue)
            {
                _logger.LogError(ue, "Unauthorized");
                await context.SetResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
