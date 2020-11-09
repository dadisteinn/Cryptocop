using System.Net;
using Cryptocop.Software.API.Models.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Cryptocop.Software.API.Extensions
{
  public static class ExceptionMiddlewareExtension
  {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
      app.UseExceptionHandler(error =>
      {
        error.Run(async context =>
        {
          var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
          var exception = exceptionHandlerFeature.Error;
          var statusCode = (int)HttpStatusCode.InternalServerError;

          if (exception is ResourceNotFoundException)
          {
            statusCode = (int)HttpStatusCode.NotFound;
          }
          else if (exception is ModelFormatException)
          {
            statusCode = (int)HttpStatusCode.PreconditionFailed;
          }
          else if (exception is InvalidSignInException)
          {
            statusCode = (int)HttpStatusCode.Unauthorized;
          }
          else if (exception is InvalidRegistrationException)
          {
            statusCode = (int)HttpStatusCode.Conflict;
          }
          else if (exception is ResourceMissingException)
          {
            statusCode = (int)HttpStatusCode.Conflict;
          }
          else if (exception is MessariException)
          {
            statusCode = (int)HttpStatusCode.ServiceUnavailable;
          }

          context.Response.ContentType = "application/json";
          context.Response.StatusCode = statusCode;

          await context.Response.WriteAsync(new ExceptionModel
          {
            StatusCode = statusCode,
            Message = exception.Message
          }.ToString());
        });
      });
    }
  }
}