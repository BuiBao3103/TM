using TM.Models;

namespace TM.Services
{
    public class AuthMiddleWare
    {
        private readonly RequestDelegate _next;


        public AuthMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (path.StartsWithSegments("/Account/Login") && context.Session.IsAvailable)
            {
                context.Response.Redirect("/Tour/Index");
                return;
            }

            // Bỏ qua những route không cần kiểm tra
            if (path.StartsWithSegments("/Account/Login") || path.StartsWithSegments("/css") || path.StartsWithSegments("/js"))
            {
                await _next(context);
                return;
            }

            // Kiểm tra session đăng nhập
            var username = context.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}
