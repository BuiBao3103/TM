using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TM.Models;
using AutoMapper;
using TM.Models.ViewModels;
using TM.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TM.Controllers
{
    public class TourController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TourController(ILogger<HomeController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        
        // GET: TourController
        //[HttpGet("List")]
        public ActionResult Index(String? name, DateTime? startDate, DateTime? endDate, int locationId = 0)
        {
            var query = _context.Tours
                    .Include(t => t.Location)
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowerName = name.Trim().ToLower();
                query = query.Where(t => t.Name.ToLower().Contains(lowerName));
            }

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

            query = query.Where(t => t.DeleteAt == null);

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
                    return RedirectToAction(nameof(Create));
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

                tour.DeleteAt = DateTime.Now;
                tour.ModifiedAt = DateTime.Now;

                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa tour thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa Tour");
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa Tour. Vui lòng thử lại.";
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: Tour/Surcharges/5
        public async Task<IActionResult> Surcharges(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.TourSurcharges.Where(s => s.DeleteAt == null))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tour == null)
            {
                return NotFound();
            }

            ViewData["TourId"] = id;
            ViewData["TourName"] = tour.Name;
            var surcharges = _mapper.Map<IEnumerable<TourSurchargeViewModel>>(tour.TourSurcharges);
            return View(surcharges);
        }

        // GET: Tour/CreateSurcharge/5
        public async Task<IActionResult> CreateSurcharge(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            var viewModel = new TourSurchargeViewModel
            {
                TourId = id,
                TourName = tour.Name
            };

            return View(viewModel);
        }

        // POST: Tour/CreateSurcharge
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSurcharge(TourSurchargeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var surcharge = _mapper.Map<TourSurcharge>(viewModel);
                surcharge.CreatedAt = DateTime.Now;

                _context.Add(surcharge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Surcharges), new { id = viewModel.TourId });
            }

            // Nếu model không hợp lệ, lấy lại tên tour để hiển thị
            var tour = await _context.Tours.FindAsync(viewModel.TourId);
            if (tour != null)
            {
                viewModel.TourName = tour.Name;
            }

            return View(viewModel);
        }
    }
}
