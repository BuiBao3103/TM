using Microsoft.AspNetCore.Mvc;
using TM.Models;
using TM.Models.Entities;

namespace TM.Controllers
{
    public class TourController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public TourController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Tour/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Tour
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                VisaDeadline = null,
                FullPayDeadline = null,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tour tour)
        {
            if (ModelState.IsValid)
            {
                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tour);
        }
    }
}
