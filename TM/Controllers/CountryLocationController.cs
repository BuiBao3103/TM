using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class CountryLocationController : Controller
    {

        private readonly AppDbContext _context;

        public CountryLocationController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? countryId)
        {
            var countries = await _context.Countries.ToListAsync();

            List<Location>? locations = null;
            if (countryId != null)
            {
                locations = await _context.Locations
                    .Where(l => l.CountryId == countryId)
                    .ToListAsync();
            }

            var viewModel = new CountryLocationViewModel
            {
                Countries = countries,
                SelectedCountryId = countryId,
                Locations = locations ?? new List<Location>()
            };

            return View(viewModel);
        }
    }
}
