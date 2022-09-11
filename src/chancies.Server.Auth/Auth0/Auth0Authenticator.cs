using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chancies.Server.Auth.Config;
using chancies.Server.Auth.Exceptions;
using chancies.Server.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace chancies.Server.Auth.Auth0
{
    /// <summary>
    /// A type that authenticates users against an Auth0 account.
    /// </summary>
    internal sealed class Auth0Authenticator
        : IAuthenticator
    {
        private readonly TokenValidationParameters _parameters;
        private readonly ConfigurationManager<OpenIdConnectConfiguration> _manager;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly ILogger<Auth0Authenticator> _logger;

        public Auth0Authenticator(
            IOptions<Auth0Config> options,
            ILogger<Auth0Authenticator> logger)
        {
            _logger = logger;

            var config = options.Value;

            _parameters = new TokenValidationParameters
            {
                ValidIssuer = $"https://{config.Domain}/",
                ValidAudiences = new [] { config.Audience },
                ValidateIssuerSigningKey = true,
            };
            _manager = new ConfigurationManager<OpenIdConnectConfiguration>($"https://{config.Domain}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
            _handler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Authenticates the user token. Returns a user principal containing claims from the token and a token that can be used to perform actions on behalf of the user.
        /// Throws an exception if the token fails to authenticate.
        /// This method has an asynchronous signature, but usually completes synchronously.
        /// </summary>
        /// <param name="authorizationValue">The authorization header value.</param>
        public async Task<(ClaimsPrincipal user, SecurityToken validatedToken)> AuthenticateAsync(string authorizationValue, params string[] permissions)
        {
            (ClaimsPrincipal user, SecurityToken token) result;

            try
            {
                var token = GetToken(authorizationValue);
                
                // Note: ConfigurationManager<T> has an automatic refresh interval of 1 day.
                //   The config is cached in-between refreshes, so this "asynchronous" call actually completes synchronously unless it needs to refresh.
                var config = await _manager.GetConfigurationAsync().ConfigureAwait(false);
                _parameters.IssuerSigningKeys = config.SigningKeys;
                var user = _handler.ValidateToken(token, _parameters, out var validatedToken);
                
                result = (user, validatedToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication failed");
                throw new UnauthorizedException();
            }

            CheckPermissions(result.user, permissions);

            return result;
        }

        /// <summary>
        /// Authenticates the user via an "Authentication: Bearer {token}" header in an HTTP request message.
        /// Returns a user principal containing claims from the token and a token that can be used to perform actions on behalf of the user.
        /// Throws an exception if the token fails to authenticate or if the Authentication header is missing or malformed.
        /// This method has an asynchronous signature, but usually completes synchronously.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="permissions">The required permissions</param>
        public Task<(ClaimsPrincipal user, SecurityToken validatedToken)> AuthenticateAsync(
            HttpRequest request,
            params string[] permissions) =>
            AuthenticateAsync(request.Headers.Authorization, permissions);
        
        private string GetToken(string authorizationValue)
        {
            var split = authorizationValue.Split(' ');
            switch (split[0])
            {
                case "Bearer":
                    return split[1];
                default:
                {
                    _logger.LogError("Invalid authorization scheme");
                    throw new UnauthorizedException();
                }
            }
        }

        private void CheckPermissions(ClaimsPrincipal claimsPrincipal, params string[] requiredPermissions)
        {
            if (requiredPermissions == null || !requiredPermissions.Any())
            {
                return;
            }

            var claimPermissions = claimsPrincipal.Claims
                .Where(c => c.Type.Equals("permissions"))
                .Select(c => c.Value)
                .ToHashSet();

            if (requiredPermissions.Any(p => !claimPermissions.Contains(p)))
            {
                throw new ForbiddenException();
            }
        }
    }
}
