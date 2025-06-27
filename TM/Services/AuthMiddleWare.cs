using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
            var pathBase = context.Request.PathBase; // Lấy sub-path như /tm
            var isLoggedIn = !string.IsNullOrEmpty(context.Session.GetString("Username"));

            // Nếu đã login mà vào lại trang login thì chuyển hướng về trang chính
            if (path.StartsWithSegments("/Account/Login") && isLoggedIn)
            {
                context.Response.Redirect(pathBase + "/Tour/Index");
                return;
            }

            // Bỏ qua kiểm tra cho các static file và trang login
            if (path.StartsWithSegments("/Account/Login") ||
                path.StartsWithSegments("/css") ||
                path.StartsWithSegments("/js") ||
                path.StartsWithSegments("/lib") ||
                path.StartsWithSegments("/images"))
            {
                await _next(context);
                return;
            }

            // Nếu chưa đăng nhập thì chuyển hướng về login
            if (string.IsNullOrEmpty(context.Session.GetString("Username")))
            {
                context.Response.Redirect(pathBase + "/Account/Login");
                return;
            }

            // Cho phép request đi tiếp
            await _next(context);
        }
    }
}
