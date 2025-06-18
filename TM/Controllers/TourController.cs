using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TM.Models;
using AutoMapper;
using TM.Models.ViewModels;
using TM.Models.Entities;

namespace TM.Controllers
{
    public class TourController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TourController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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