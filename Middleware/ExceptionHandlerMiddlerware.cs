using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TailwindApi.Middleware
{
    public class ExceptionHandlerMiddlerware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddlerware> _logger;

        public ExceptionHandlerMiddlerware(ILogger<ExceptionHandlerMiddlerware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline.
                await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // Log This Exception 
                _logger.LogError(ex, $"{errorId} THIS IS EXCEPTION {ex.Message}");

                // Return A Custom Error Resoponse

                var httpReqData = await context.GetHttpRequestDataAsync();

                if (httpReqData != null)
                {
                    var httpResponse = httpReqData.CreateResponse(HttpStatusCode.InternalServerError);
                    httpResponse.Headers.Add("Content-Type", "application/json");
                    var error = new
                    {
                        Id = errorId,
                        ErrorMessage = "Somthing went wrong!"
                    };
                    await httpResponse.WriteAsJsonAsync(error);
                }
            }
        }

    }
}
