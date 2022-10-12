using System;
using System.Net;
using System.Threading.Tasks;
using chancies.Server.Api.FunctionApp.Extensions;
using chancies.Server.Auth.Exceptions;
using chancies.Server.Common.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using UnauthorizedException = chancies.Server.Auth.Exceptions.UnauthorizedException;

namespace chancies.Server.Api.FunctionApp.Middleware
{
    internal class ExceptionHandlerMiddleware
        : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AggregateException ex)
            {
                HttpStatusCode statusCode;
                object data = null;
                
                switch (ex.InnerException)
                {
                    case DuplicateObjectException:
                        statusCode = HttpStatusCode.Conflict;
                        break;
                    case ForbiddenException:
                        statusCode = HttpStatusCode.Forbidden;
                        break;
                    case UnauthorizedException:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                    case NotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        break;
                    case InvalidDataException:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case InUseException iue:
                        statusCode = HttpStatusCode.Conflict;
                        data = new { Error = iue.Message };
                        break;
                    default:
                        throw;
                }

                _logger.LogError(ex, $"Http response: {statusCode}");
                await context.SetResponse(statusCode, data);
            }
        }   
    }
}
