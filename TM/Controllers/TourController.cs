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
                    .Include(t => t.Location) // <--- Load luôn thông tin Location
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

            var tours = query.ToList();
            return View(tours);
        }
      

        public IActionResult Details(int id)
        {
            var tour = _context.Tours.Find(id);
            if (tour == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Chi tiết tour";
            return View(tour);
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
                    return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.TourSurcharges.Where(s => s.DeleteAt == null))
                .Include(t => t.Passengers.Where(p => p.DeleteAt == null))
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null)
            {
                return NotFound();
            }

            // Query locations với country info
            var locations = _context.Locations
                .Include(l => l.Country)
                .ToList()
                .Select(l => new
                {
                    Id = l.Id,
                    DisplayText = $"{l.LocationName} - {l.Country.Name}"
                })
                .OrderBy(l => l.DisplayText)
                .ToList();

            ViewBag.LocationId = new SelectList(locations, "Id", "DisplayText");

            // Map surcharges và passengers sang ViewModel
            ViewData["Surcharges"] = _mapper.Map<IEnumerable<TourSurchargeViewModel>>(tour.TourSurcharges);
            ViewData["Passengers"] = tour.Passengers;

            return View(tour);
        }

        // POST: TourController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tour tour)
        {
            if (id != tour.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tour.ModifiedAt = DateTime.Now;
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Nếu model không hợp lệ, load lại locations cho dropdown
            var locations = _context.Locations
                .Include(l => l.Country)
                .ToList()
                .Select(l => new
                {
                    Id = l.Id,
                    DisplayText = $"{l.LocationName} - {l.Country.Name}"
                })
                .OrderBy(l => l.DisplayText)
                .ToList();

            ViewBag.LocationId = new SelectList(locations, "Id", "DisplayText");

            return View(tour);
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.Id == id);
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
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa Tour");
                // Có thể trả về view với thông báo lỗi hoặc chuyển hướng về List với TempData
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
