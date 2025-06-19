using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public enum PassengerGender
    {
        [Display(Name = "Nam")]
        Male,

        [Display(Name = "Nữ")]
        Female,

        [Display(Name = "Khác")]
        Other
    }
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
        [HttpGet]
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

            query = query.Where(t => t.DeleteAt == null);
            
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

            return View();
        }

        // POST: TourController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
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

                ViewBag.LocationId = new SelectList(locations, "Id", "DisplayText", model.LocationId);

                return View(model);
            }

            try
            {
                var tour = _mapper.Map<Tour>(model);
                tour.AvailableSeats = tour.TotalSeats;
                tour.CreatedAt = DateTime.Now;
                tour.ModifiedAt = DateTime.Now;

                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới Tour");

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

                ViewBag.LocationId = new SelectList(locations, "Id", "DisplayText", model.LocationId);

                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo Tour. Vui lòng thử lại.");
                return View(model);
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

        [Route("TourController/Passengers/Get")]
        public IActionResult Passengers()
        {
            //var tour = _context.Tours.FirstOrDefault(t => t.Id == id);
            //var passengers = _context.Passengers.ToList();
            return View();
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
                return  Redirect("/Tour/Edit/" + viewModel.TourId);
            }

            // Nếu model không hợp lệ, lấy lại tên tour để hiển thị
            var tour = await _context.Tours.FindAsync(viewModel.TourId);
            if (tour != null)
            {
                viewModel.TourName = tour.Name;
            }

            return View(viewModel);
        }

        // POST: Tour/AddTourPassenger
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTourPassenger(TourPassengerViewModel viewModel)
        {

            if (viewModel.Code.IsNullOrEmpty())
            {
                ModelState.AddModelError("Code", "Mã không được để trống.");
                return View(viewModel);
            }

            if (viewModel.FullName.IsNullOrEmpty())
            {
                ModelState.AddModelError("FullName", "Họ và tên không được để trống.");
                return View(viewModel);
            }

            if (!Enum.TryParse<PassengerGender>(viewModel.Gender, out _))
            {
                ModelState.AddModelError("Gender", "Giới tính không hợp lệ.");
                return View(viewModel);
            }

            if (!Regex.IsMatch(viewModel.Phone ?? "", @"^\d+$"))
            {
                ModelState.AddModelError("Phone", "Số điện thoại chỉ được chứa chữ số.");
                return View(viewModel);
            }

            if (!Regex.IsMatch(viewModel.IdentityNumber ?? "", @"^\d+$"))
            {
                ModelState.AddModelError("IdentityNumber", "Số CCCD chỉ được chứa chữ số.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {

                bool emailExists = _context.Passengers.Any(p => p.Email == viewModel.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email này đã tồn tại.");
                    return View(viewModel);
                }

                bool phoneExists = _context.Passengers.Any(p => p.Phone == viewModel.Phone);
                if (phoneExists)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại này đã tồn tại.");
                    return View(viewModel);
                }

                bool identityNumberExists = _context.Passengers.Any(p => p.IdentityNumber == viewModel.IdentityNumber);
                if (identityNumberExists)
                {
                    ModelState.AddModelError("IdentityNumber", "Số điện thoại này đã tồn tại.");
                    return View(viewModel);
                }

                Tour? tourUpdate = await _context.Tours.FindAsync(viewModel.TourId);

                Passenger? passenger = _mapper.Map<Passenger>(viewModel);
                passenger.CreatedAt = DateTime.Now;
                passenger.TourId= viewModel.TourId;
                _context.Add(passenger);

                int bookedSeatsAmount = _context.Passengers.Where(p => p.TourId == viewModel.TourId).ToList().Count;

                tourUpdate.AvailableSeats = tourUpdate.TotalSeats - bookedSeatsAmount;

                if (tourUpdate.AvailableSeats == 0)
                {
                    ModelState.AddModelError("", "Tour đã đủ số lượng hành khách.");
                    return View(viewModel);
                }

                await _context.SaveChangesAsync();
                return Redirect("/Tour/Edit/" + viewModel.TourId);
            }

            // Nếu model không hợp lệ, lấy lại tên tour để hiển thị
            var tour = await _context.Tours.FindAsync(viewModel.TourId);
            if (tour != null)
            {
                viewModel.TourName = tour.Name;
            }

            return View(viewModel);
        }

        public async Task<IActionResult> AddTourPassenger(int tourId)
        {
            var tour = await _context.Tours.FindAsync(tourId);
            if (tour == null)
            {
                return NotFound();
            }

            var viewModel = new TourPassengerViewModel
            {
                TourId = tourId,
                TourName = tour.Name,
                TourCode = tour.Code,
                AssignedPrice = tour.DiscountPrice ?? 0,
                DateOfBirth = new DateTime(2000, 1, 1)

            };

            return View(viewModel);
            }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePassenger(int id)
        {
            var passenger = _context.Passengers.FirstOrDefault(p => p.Id == id && p.DeleteAt == null);
            if (passenger != null)
            {
                passenger.DeleteAt = DateTime.Now;
                _context.SaveChanges();
            }

            return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
        }

        public IActionResult EditPassenger(int id)
        {
            var passenger = _context.Passengers.Find(id);
            if (passenger == null)
            {
                return NotFound();
            }
            var viewModel = new PassengerViewModel
            {
                Id = passenger.Id,
                FullName = passenger.FullName,
                Code = passenger.Code,
                DateOfBirth = passenger.DateOfBirth,
                Gender = passenger.Gender,
                IdentityNumber = passenger.IdentityNumber ?? "",
                Phone = passenger.Phone,
                Email = passenger.Email,
                Address = passenger.Address,
                TourId = passenger.TourId,
                AssignedPrice = passenger.AssignedPrice,
                CustomerPaid = passenger.AssignedPrice,
                Status = passenger.Status,
            };
            ViewData["Title"] = "Sửa thông tin khách hàng";

            return View(viewModel); 
        }
        [HttpPost]
        public IActionResult UpdatePassenger(PassengerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditPassenger", model); 
            }
            var passenger = _context.Passengers.Find(model.Id);
            if (passenger == null)
            {
                return NotFound();
            }
            passenger.FullName = model.FullName;
            passenger.Code = model.Code;
            passenger.DateOfBirth = model.DateOfBirth;
            passenger.Gender = model.Gender;
            passenger.IdentityNumber = model.IdentityNumber;
            passenger.Phone = model.Phone;
            passenger.Email = model.Email;
            passenger.Address = model.Address;
            passenger.TourId = model.TourId;
            passenger.AssignedPrice = model.AssignedPrice;
            passenger.CustomerPaid = model.CustomerPaid;
            passenger.Status = model.Status;

            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = model.TourId });
        }
    }
}
