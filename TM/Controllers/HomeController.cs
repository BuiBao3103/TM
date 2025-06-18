using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TM.Models;

namespace TM.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Thêm lại action Index
        public IActionResult Index()
        {
            return View();
        }

        // Action test kết nối database
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Đặt timeout cho kết nối (ví dụ: 5 giây)
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    connection.OpenAsync().Wait(TimeSpan.FromSeconds(5));

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        ViewBag.Message = "Kết nối database thành công!";
                        ViewBag.Status = "success";
                    }
                    else
                    {
                        ViewBag.Message = "Không thể kết nối database!";
                        ViewBag.Status = "error";
                    }
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = $"Lỗi SQL: {ex.Message} (Error Number: {ex.Number})";
                ViewBag.Status = "error";
            }
            catch (TimeoutException)
            {
                ViewBag.Message = "Kết nối database bị timeout!";
                ViewBag.Status = "error";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Lỗi khác: {ex.Message} {_context.Database.GetConnectionString()}";
                ViewBag.Status = "error";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

