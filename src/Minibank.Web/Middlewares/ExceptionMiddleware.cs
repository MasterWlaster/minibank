using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minibank.Core.Exceptions;

namespace Minibank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate Next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await Next(httpContext);
            }
            catch (ValidationException e)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = e.Message });
            }
            catch (FluentValidation.ValidationException e)
            {
                var errors = e.Errors
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}");

                var errorMessage = string.Join(Environment.NewLine, errors);

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(errorMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Message = "Internal server error" });
            }
        }
    }
}
