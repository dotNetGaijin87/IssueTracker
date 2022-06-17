using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using IssueTracker.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IssueTracker.ServicesExtensions;

public static class ProblemDetailsExt
{
    public static IServiceCollection AddProblemDetailsExt(this IServiceCollection services)
    {
        return services.AddProblemDetails(opts =>
        {
            opts.IncludeExceptionDetails = (context, ex) =>
            {
                var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
                return environment.IsDevelopment();
            };
           
            opts.Map<ExceptionWithStatusCode>(exception => new ProblemDetails
            {
                Title = "Error",
                Detail = exception.Message,
                Status = (int)exception.StatusCode,
                Type = exception.GetType().ToString()
            });


            opts.Map<ValidationException>(exception => new ProblemDetails
            {
                Title = "Validation Error",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.BadRequest,
                Type = exception.GetType().ToString()
            });
        });
    }
}


