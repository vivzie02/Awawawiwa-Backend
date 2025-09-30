using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
    {
        string correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault()
                               ?? Guid.NewGuid().ToString();

        context.Items[CorrelationHeader] = correlationId;
        context.Response.Headers[CorrelationHeader] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await _next(context);
        }
    }
}
