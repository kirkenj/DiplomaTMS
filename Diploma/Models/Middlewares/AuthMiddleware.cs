//using Microsoft.Extensions.Primitives;
//using System.Net.Http.Headers;
//using System.Text;
//using static Diploma.Models.Constants.Constants;

//namespace Diploma.Models.Middlewares
//{
//    public class AuthMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public AuthMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }


//        public async Task InvokeAsync(HttpContext context)
//        {
//            if (context.Request.Cookies.ContainsKey(TokenCookiesKey) && context.Request.Cookies[TokenCookiesKey] != null)
//            {
//                var a = context.Request.Headers.Authorization.Append(context.Request.Cookies[TokenCookiesKey]);
//                a = a;

//                //context.Request.Headers.Authorization = (StringValues);
//            }

//            await _next(context);
//        }
//    }
//}
