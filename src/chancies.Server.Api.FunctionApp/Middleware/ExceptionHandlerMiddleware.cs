using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace chancies.Server.Api.FunctionApp.Middleware
{
    internal class ExceptionHandlerMiddleware
        : IFunctionsWorkerMiddleware
    {
        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
