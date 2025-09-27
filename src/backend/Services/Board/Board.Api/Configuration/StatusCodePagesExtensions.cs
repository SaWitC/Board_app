using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Board.Api.Configuration;

public static class StatusCodePagesExtensions
{
    public static IApplicationBuilder UseProblemDetailsForStatusCodes(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(async context =>
        {
            HttpContext httpContext = context.HttpContext;
            HttpResponse response = httpContext.Response;

            if (response.HasStarted)
            {
                return;
            }

            if (response.StatusCode == StatusCodes.Status204NoContent || response.StatusCode == StatusCodes.Status304NotModified)
            {
                return;
            }

            IProblemDetailsService problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

            ProblemDetails pd = new ProblemDetails
            {
                Status = response.StatusCode,
                Title = ReasonPhrases.GetReasonPhrase(response.StatusCode),
                Type = "about:blank",
                Instance = httpContext.Request.Path
            };

            response.ContentType = "application/problem+json";
            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = pd
            });
        });

        return app;
    }
}
