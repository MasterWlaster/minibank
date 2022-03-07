using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Minibank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { Message = "Internal server error" });
            }
        }
    }
}
