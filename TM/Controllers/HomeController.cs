using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TM.Models;

namespace TM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Countries.ToList();
            return View();
        }
   
        public IActionResult Privacy()
        {
            return View();
        }


        [Route("Home/TourDetails/{id}")]
        public IActionResult TourDetails(int id)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.Id == id);
            return View(tour);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
