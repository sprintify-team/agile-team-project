using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {

        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {
                        // Exception türüne göre HTTP status set et
                        var statusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            ArgumentException => StatusCodes.Status400BadRequest,
                            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        context.Response.StatusCode = statusCode;

                        // Hata modelini döndür
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = statusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }

    }
}
