using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
               await next(context);
            }
            catch (NotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status308PermanentRedirect;  
            }
            catch (ResultFailedException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
        }
    }
}
