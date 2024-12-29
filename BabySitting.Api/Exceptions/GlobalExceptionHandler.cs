using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        httpContext.Response.StatusCode = exception switch
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        // activity?.Id is used for telemetry purposes...
        // the traceId can potentialy be used for distributed tracing systems (DataDog, Jaeger, Aspire Dashboard) to find the logs of the request
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Extensions = new Dictionary<string, object?>
                {
                    { "requestId", httpContext.TraceIdentifier},
                    { "traceId", activity?.Id }
                }
            }
        });
    }
}
