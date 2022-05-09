using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minibank.Core.Exceptions;

namespace Minibank.Web.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        public readonly RequestDelegate Next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var tokenString = httpContext.Request.Headers["Authorization"].ToString();
            tokenString = tokenString.Substring(7, tokenString.Length - 7);
            
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenString);

            var token = jsonToken as JwtSecurityToken;

            var exp = Convert.ToInt32(token?.Claims.First(claim => claim.Type == "exp").Value);
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (exp <= now)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsJsonAsync(new {Message = "invalid token"});
                
                return;
            }

            await Next(httpContext);
        }
    }
}
