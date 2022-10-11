using System;
using System.Net;
using System.Threading.Tasks;
using chancies.Server.Auth.Exceptions;
using chancies.Server.Common.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using UnauthorizedException = chancies.Server.Auth.Exceptions.UnauthorizedException;

namespace chancies.Server.Api.FunctionApp.Middleware
{
    internal class ExceptionHandlerMiddleware
        : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UnauthorizedException)
            {
                var requestData = await context.GetHttpRequestDataAsync();
                var response = requestData!.CreateResponse();
                response.StatusCode = HttpStatusCode.Unauthorized;
                context.GetInvocationResult().Value = response;
            }
            catch (AggregateException ex)
            {
                var requestData = await context.GetHttpRequestDataAsync();
                var response = requestData!.CreateResponse();

                switch (ex.InnerException)
                {
                    case ForbiddenException:
                        response.StatusCode = HttpStatusCode.Forbidden;
                        break;
                    case UnauthorizedException:
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        break;
                    case NotFoundException:
                        response.StatusCode = HttpStatusCode.NotFound;
                        break;
                    default:
                        throw;
                }

                context.GetInvocationResult().Value = response;
            }
        }   
    }
}
