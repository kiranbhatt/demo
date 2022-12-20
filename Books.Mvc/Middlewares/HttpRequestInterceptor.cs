using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Books.Mvc.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HttpRequestInterceptor
    {
        private readonly RequestDelegate _next;

        public HttpRequestInterceptor(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var JWToken = httpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(JWToken))
            {
                httpContext.Request.Headers.Add("Authorization", "Bearer " + JWToken);
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpRequestInterceptorExtensions
    {
        public static IApplicationBuilder UseHttpRequestInterceptor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpRequestInterceptor>();
        }
    }
}
