using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        
        // GET: TourController
        //[HttpGet("List")]
        public ActionResult Index(DateTime? startDate, DateTime? endDate, int locationId = 0)
        {
            var query = _context.Tours
                    .Include(t => t.Location) // <--- Load luôn thông tin Location
                    .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.EndDate <= endDate.Value);
            }

            if (locationId != 0)
            {
                query = query.Where(t => t.LocationId == locationId);
            }

            var tours = query.ToList();
            return View(tours);
        }
        // GET: TourController
        public ActionResult List()
        {
            var tours = _context.Tours.ToList();
            return View(tours);
        }

        // GET: TourController/Details/5
        public ActionResult Details(int id)
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

            // Query locations với country info
            var locations = _context.Locations
                .Include(l => l.Country)
                .ToList() // Execute query first
                .Select(l => new
                {
                    Id = l.Id,
                    DisplayText = $"{l.LocationName} - {l.Country.Name}"
                })
                .OrderBy(l => l.DisplayText)
                .ToList();

            ViewBag.LocationId = new SelectList(locations, "Id", "DisplayText");


            return View(model);
        }

        // POST: TourController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tour tour)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Tours.Add(tour);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
                }
                return View(tour);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới Tour");
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo Tour. Vui lòng thử lại.");
                return View(tour);
            }
        }

        // GET: TourController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TourController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TourController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TourController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tour = await _context.Tours.FindAsync(id);
                if (tour == null)
                {
                    return NotFound();
                }

                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa Tour");
                // Có thể trả về view với thông báo lỗi hoặc chuyển hướng về List với TempData
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa Tour. Vui lòng thử lại.";
                return RedirectToAction(nameof(List));
            }
        }
    }
}
