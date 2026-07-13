using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Revision_of_Data_Seeding.CustomExcptions
{
    public class GlobalExceptionHanlder : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statuscode, title) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
                _ => (StatusCodes.Status500InternalServerError, "Server Error")
            };

            httpContext.Response.StatusCode = statuscode;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statuscode,
                Title = title,
                Detail = exception.Message
            }, cancellationToken);


            return true;
        }
    }
}
