using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class TourController : Controller
    {
        private readonly AppDbContext _context;

        public TourController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TourController
        //[HttpGet("List")]
        public async Task<IActionResult> Index(
            string? name,
            DateTime? startDate,
            DateTime? endDate,
            int? countryId,
            int? locationId)
        {
            var countries = await _context.Countries.ToListAsync();
            var locations = new List<Location>();

            if (countryId.HasValue)
            {
                locations = await _context.Locations
                    .Where(l => l.CountryId == countryId)
                    .ToListAsync();
            }

            var query = _context.Tours
                .Include(t => t.Location)
                .ThenInclude(l => l.Country)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowerName = name.Trim().ToLower();
                query = query.Where(t => t.Name.ToLower().Contains(lowerName));
            }

            if (startDate.HasValue)
                query = query.Where(t => t.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.EndDate <= endDate.Value);

            if (locationId.HasValue && locationId.Value != 0)
                query = query.Where(t => t.LocationId == locationId);
            else if (countryId.HasValue)
                query = query.Where(t => t.Location!.CountryId == countryId);

            var model = new TourViewModel
            {
                Countries = countries,
                SelectedCountryId = countryId,
                Locations = locations,
                SelectedLocationId = locationId,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                Tours = await query.ToListAsync()
            };

            return View(model);
        }


        // GET: TourController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TourController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TourController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
