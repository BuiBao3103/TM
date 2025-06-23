using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;
using TM.Services;

namespace TM.Controllers
{
    public class TourController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly PassengerValidatorService _validator;

        public TourController(ILogger<HomeController> logger, AppDbContext context, IMapper mapper, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _validator = new PassengerValidatorService(context);
        }

        // GET: TourController
        [HttpGet]
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> Index(
            string? name,
            DateTime? startDate,
            DateTime? endDate,
            int? countryId,
            int? locationId,
            decimal? minPrice,
            decimal? maxPrice,
            string? status,
            bool? isVisaRequired,
            int page = 1,
            int pageSize = 12)
        {
            var countries = await _context.Countries.ToListAsync();
            var locations = new List<Location>();

            if (countryId.HasValue)
            {
                locations = await _context.Locations
                    .Where(l => l.CountryId == countryId)
                    .ToListAsync();
            }

            var queryTour = from t in _context.Tours
                            join l in _context.Locations on t.LocationId equals l.Id
                            join c in _context.Countries on l.CountryId equals c.Id
                            join p in _context.Passengers
                                .Where(p => p.DeleteAt == null && (p.Status != "Cancelled" || p.Status == null))
                                on t.Id equals p.TourId into passengerGroup
                            from p in passengerGroup.DefaultIfEmpty()
                            where t.DeleteAt == null
                            group new { t, l, c, p } by new
                            {
                                t.Id,
                                l.LocationName,
                                CountryName = c.Name,
                                TourName = t.Name,
                                t.StartDate,
                                t.EndDate,
                                t.SuggestPrice,
                                t.DiscountPrice,
                                t.AvailableSeats,
                                t.TotalSeats,
                                t.Status,
                                t.IsVisaRequired
                            } into g
                            select new TourWithPassengerStatsViewModel
                            {
                                Id = g.Key.Id,
                                LocationName = g.Key.LocationName,
                                CountryName = g.Key.CountryName,
                                Name = g.Key.TourName,
                                StartDate = g.Key.StartDate,
                                EndDate = g.Key.EndDate,
                                SuggestPrice = g.Key.SuggestPrice,
                                DiscountPrice = g.Key.DiscountPrice,
                                AvailableSeats = g.Key.AvailableSeats,
                                TotalSeats = g.Key.TotalSeats,
                                Status = g.Key.Status,
                                IsVisaRequired = g.Key.IsVisaRequired,
                                TotalCustomers = g.Count(x => x.p != null),
                                FullPayCustomers = g.Count(x => x.p != null && x.p.AssignedPrice <= x.p.CustomerPaid),
                                CustomerNoPassport = g.Count(x => x.p != null && string.IsNullOrEmpty(x.p.PassportNum)),
                                ReservedCustomer = g.Count(x => x.p != null && x.p.Status == "Reserved"),
                                DepositedCustomer = g.Count(x => x.p != null && x.p.CustomerPaid > 0 && x.p.CustomerPaid < x.p.AssignedPrice),
                                CustomerFullPayNotTicket = g.Count(x => x.p != null && x.p.AssignedPrice <= x.p.CustomerPaid &&
                                    (string.IsNullOrEmpty(x.p.DepartureFlightInfo) || string.IsNullOrEmpty(x.p.ArrivalFlightInfo)))
                            };
            // Áp dụng các điều kiện lọc
            if (!string.IsNullOrWhiteSpace(name))
            {
                string lowerName = name.Trim().ToLower();
                queryTour = queryTour.Where(g => g.Name.ToLower().Contains(lowerName));
            }

            if (startDate.HasValue)
            {
                queryTour = queryTour.Where(g => g.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                queryTour = queryTour.Where(g => g.EndDate <= endDate.Value);
            }

            if (locationId.HasValue && locationId.Value != 0)
            {
                queryTour = queryTour.Where(g => g.Id == locationId.Value);
            }
            else if (countryId.HasValue)
            {
                queryTour = queryTour.Where(g => g.CountryName == _context.Countries
                    .Where(c => c.Id == countryId.Value)
                    .Select(c => c.Name)
                    .FirstOrDefault());
            }

            if (minPrice.HasValue)
            {
                queryTour = queryTour.Where(g => (g.DiscountPrice ?? g.SuggestPrice) >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                queryTour = queryTour.Where(g => (g.DiscountPrice ?? g.SuggestPrice) <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                queryTour = queryTour.Where(g => g.Status == status);
            }

            if (isVisaRequired.HasValue)
            {
                queryTour = queryTour.Where(g => g.IsVisaRequired == isVisaRequired.Value);
            }

            // pagination
            queryTour = queryTour.OrderBy(t => t.Id);
            var totalRecords = await queryTour.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var skip = (page - 1) * pageSize;
            var listTour = await queryTour.Skip(skip).Take(pageSize).ToListAsync();

            // create pagin model
            var paginViewModel = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalRecords,
                ActionName = ""
            };

            TourViewModel model = new TourViewModel
            {
                Countries = countries,
                SelectedCountryId = countryId,
                Locations = locations,
                SelectedLocationId = locationId,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Status = status,
                IsVisaRequired = isVisaRequired,
                Tours = listTour,
                Pagination = paginViewModel
            };

            return View(model);
        }


        [RequireAuthorize("Admin", "Sale")]
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

        [RequireAuthorize("Admin", "Sale")]
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
                Tour? tour = _mapper.Map<Tour>(model);
                tour.AvailableSeats = tour.TotalSeats;
                tour.CreatedAt = DateTime.Now;
                tour.ModifiedAt = DateTime.Now;

                _context.Tours.Add(tour);
                await _context.SaveChangesAsync();

                if (tour.FullPayDeadline.HasValue)
                {
                    TimeSpan delay = tour.FullPayDeadline.Value - DateTime.Now;

                    if (delay > TimeSpan.Zero)
                    {
                        _backgroundJobClient.Schedule<TM.Services.PassengerStatusChecker>(
                            checker => checker.CheckHoldTime(tour.Id),
                            delay
                        );
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
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

                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo Tour. Vui lòng thử lại.");
                return View(model);
            }
        }

        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> Details(int id)
        {
            var query = from t in _context.Tours
                        join l in _context.Locations on t.LocationId equals l.Id
                        join c in _context.Countries on l.CountryId equals c.Id
                        join p in _context.Passengers on t.Id equals p.TourId into passengerGroup
                        from p in passengerGroup.DefaultIfEmpty()
                        where t.DeleteAt == null
                            && (p.Status != "Cancelled" || p.Status == null)
                            && p.DeleteAt == null
                            && t.Id == id
                        group new { t, l, c, p } by new
                        {
                            t.Id,
                            l.LocationName,
                            CountryName = c.Name,
                            t.Name,
                            t.StartDate,
                            t.EndDate,
                            t.SuggestPrice,
                            t.DiscountPrice,
                            t.AvailableSeats,
                            t.TotalSeats,
                            t.Status,
                            t.IsVisaRequired,
                            t.Code,
                            t.HhFee,
                            t.DepartureFlightInfo,
                            t.ArrivalFlightInfo,
                            t.IsAutoHoldTime,
                            t.HoldTime,
                            t.VisaDeadline,
                            t.FullPayDeadline,
                            t.LocationId,
                            t.DepartureLocation,
                            t.DepartureAssembleTime,
                            t.RoomNote,
                            t.Note,
                            t.CreatedAt,
                            t.ModifiedAt,
                            t.ModifiedById,
                            t.DeleteAt
                        } into g
                        select new TourWithPassengerStatsViewModel
                        {
                            // Tour properties
                            Id = g.Key.Id,
                            LocationName = g.Key.LocationName,
                            CountryName = g.Key.Name,
                            Name = g.Key.Name,
                            StartDate = g.Key.StartDate,
                            EndDate = g.Key.EndDate,
                            SuggestPrice = g.Key.SuggestPrice,
                            DiscountPrice = g.Key.DiscountPrice,
                            AvailableSeats = g.Key.AvailableSeats,
                            TotalSeats = g.Key.TotalSeats,
                            Status = g.Key.Status,
                            IsVisaRequired = g.Key.IsVisaRequired,
                            Code = g.Key.Code,
                            HhFee = g.Key.HhFee,
                            DepartureFlightInfo = g.Key.DepartureFlightInfo,
                            ArrivalFlightInfo = g.Key.ArrivalFlightInfo,
                            IsAutoHoldTime = g.Key.IsAutoHoldTime,
                            HoldTime = g.Key.HoldTime,
                            VisaDeadline = g.Key.VisaDeadline,
                            FullPayDeadline = g.Key.FullPayDeadline,
                            LocationId = g.Key.LocationId,
                            DepartureLocation = g.Key.DepartureLocation,
                            DepartureAssembleTime = g.Key.DepartureAssembleTime,
                            RoomNote = g.Key.RoomNote,
                            Note = g.Key.Note,
                            CreatedAt = g.Key.CreatedAt,
                            ModifiedAt = g.Key.ModifiedAt,
                            ModifiedById = g.Key.ModifiedById,
                            DeleteAt = g.Key.DeleteAt,
                            // Aggregated fields
                            TotalCustomers = g.Count(x => x.p != null && x.p.Id != 0),
                            FullPayCustomers = g.Count(x => x.p != null && x.p.AssignedPrice <= x.p.CustomerPaid),
                            CustomerNoPassport = g.Count(x => x.p != null && (x.p.PassportNum == null || x.p.PassportNum == "")),
                            ReservedCustomer = g.Count(x => x.p != null && x.p.Status == "Reserved"),
                            DepositedCustomer = g.Count(x => x.p != null && x.p.CustomerPaid > 0 && x.p.CustomerPaid < x.p.AssignedPrice),
                            CustomerFullPayNotTicket = g.Count(x => x.p != null && x.p.AssignedPrice <= x.p.CustomerPaid
                                && (x.p.DepartureFlightInfo == null || x.p.DepartureFlightInfo == ""
                                    || x.p.ArrivalFlightInfo == null || x.p.ArrivalFlightInfo == "")),
                            TotalAssignedPrice = g.Sum(x => x.p != null ? x.p.AssignedPrice : 0),
                            TotalCustomerPaid = g.Sum(x => x.p != null ? x.p.CustomerPaid : 0),
                        };

            var tour = await query.FirstOrDefaultAsync();

            if (tour == null)
            {
                return NotFound();
            }

            var tourSurcharges = await _context.TourSurcharges.Where(s => s.TourId == id && s.DeleteAt == null).ToListAsync();
            var tourPassengers = await _context.Passengers.Where(p => p.TourId == id && p.DeleteAt == null).ToListAsync();

            tour.Passengers = tourPassengers;
            tour.TourSurcharges = tourSurcharges;

            ViewData["Title"] = "Chi tiết tour";
            return View(tour);
        }

        [HttpGet]
        [RequireAuthorize("Admin", "Sale")]
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

            var model = _mapper.Map<TourEditViewModel>(tour);
            ViewData["Surcharges"] = _mapper.Map<IEnumerable<TourSurchargeViewModel>>(tour.TourSurcharges);
            ViewData["Passengers"] = _mapper.Map<List<PassengerViewModel>>(tour.Passengers);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> Edit(int id, TourEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

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

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            _mapper.Map(model, tour);
            tour.ModifiedAt = DateTime.Now;
            _context.Update(tour);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sửa thành công";

            return RedirectToAction("Edit");
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin", "Sale")]
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
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa Tour. Vui lòng thử lại.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("TourController/Passengers/Get")]
        [RequireAuthorize("Admin", "Sale")]
        public IActionResult Passengers()
        {
            //var tour = _context.Tours.FirstOrDefault(t => t.Id == id);
            //var passengers = _context.Passengers.ToList();
            return View();
        }

        // Passengers handle section
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> AddTourPassenger(int tourId)
        {
            Tour? tour = await _context.Tours.FindAsync(tourId);
            if (tour == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, dữ liệu không tồn tại.";
                return Redirect($"{Url.Action("Edit", new { id = tourId })}#passenger-list");
            }

            var viewModel = new PassengerAddViewModel
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
        [RequireAuthorize("Admin", "Sale")]
        public async Task<IActionResult> AddTourPassenger(PassengerAddViewModel viewModel)
        {
            // Check duplicate identity/passport/code in tour
            _validator.ValidateDuplicatePassengerFields(
                viewModel.Id,
                viewModel.TourId,
                viewModel.IdentityNumber,
                viewModel.Code,
                viewModel.PassportNum,
                ModelState
            );

            // If model state is invalid, retrieve the tour name and return the view with the model
            if (!ModelState.IsValid)
            {
                Tour? tour = await _context.Tours.FindAsync(viewModel.TourId);
                if (tour != null)
                {
                    viewModel.TourName = tour.Name;
                    viewModel.TourCode = tour.Code;
                }
                return View(viewModel);
            }

            // Check if passenger is existed 
            Passenger? passenger = _mapper.Map<Passenger>(viewModel);
            if (passenger == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, dữ liệu không tồn tại.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            // Create a new passenger
            passenger.CreatedAt = DateTime.Now;
            passenger.TourId = viewModel.TourId;
            passenger.ModifiedAt = DateTime.Now;
            if (int.TryParse(HttpContext.Session.GetString("AuthId"), out int authId))
                passenger.ModifiedById = authId;

            _context.Add(passenger);

            await _context.SaveChangesAsync();

            // Update tour's available seats
            Tour? tourUpdate = await _context.Tours.FindAsync(viewModel.TourId);
            int bookedSeatsAmount = _context.Passengers
                .Where(p => p.TourId == viewModel.TourId
                            && p.Status != Enum.PassengerStatus.Cancelled.ToString())
                .ToList()
                .Count;

            if (tourUpdate == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, dữ liệu không tồn tại.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            tourUpdate.AvailableSeats = tourUpdate.TotalSeats - bookedSeatsAmount;

            if (tourUpdate.AvailableSeats == 0)
            {
                ModelState.AddModelError("", "Tour đã đủ số lượng hành khách.");
                return View(viewModel);
            }

            await _context.SaveChangesAsync();

            if (passenger.Status == Enum.PassengerStatus.Reserved.ToString() && tourUpdate.IsAutoHoldTime == true && tourUpdate.HoldTime.HasValue)
            {
                _backgroundJobClient.Schedule<PassengerStatusChecker>(
                    checker => checker.CheckHoldTime(passenger.Id),
                    TimeSpan.FromHours(tourUpdate.HoldTime.Value)
                //TimeSpan.FromSeconds(3)
                );
            }

            TempData["SuccessMessage"] = "Thêm hành khách thành công!";
            return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
        }

        [RequireAuthorize("Admin", "Sale")]
        public IActionResult EditPassenger(int passengerId)
        {
            Passenger? passenger = _context.Passengers.Find(passengerId);
            if (passenger == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, dữ liệu không tồn tại.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            String? id = HttpContext.Session.GetString("AuthId");
            String? role = HttpContext.Session.GetString("Role");

            bool isAdmin = role == "Admin";
            bool isValidSale = role == "Sale" && passenger?.ModifiedById != null && passenger.ModifiedById.ToString() == id;

            if (!(isAdmin || isValidSale))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xóa hành khách này.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            PassengerEditViewModel viewModel = _mapper.Map<TM.Models.ViewModels.PassengerEditViewModel>(passenger);
            ViewData["Title"] = "Sửa thông tin khách hàng";
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin", "Sale")]
        public IActionResult EditPassenger(PassengerEditViewModel viewModel)
        {
            // Check duplicate identity/passport/code in tour
            _validator.ValidateDuplicatePassengerFields(
                viewModel.Id,
                viewModel.TourId,
                viewModel.IdentityNumber,
                viewModel.Code,
                viewModel.PassportNum,
                ModelState
            );

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Passenger? passenger = _context.Passengers.Find(viewModel.Id);
            if (passenger == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, dữ liệu không tồn tại.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            _mapper.Map(viewModel, passenger);
            _context.SaveChanges();

            Tour? tour = _context.Tours.Find(passenger?.TourId);

            if (tour != null)
            {
                int bookedSeatsAmount = _context.Passengers
                .Where(p => p.TourId == viewModel.TourId
                            && p.Status != Enum.PassengerStatus.Cancelled.ToString())
                .ToList()
                .Count;
                tour.AvailableSeats = tour.TotalSeats - bookedSeatsAmount;
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Cập nhật hành khách thành công!";
            return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin", "Sale")]
        public IActionResult DeletePassenger(int passengerId)
        {
            Passenger? passenger = _context.Passengers.FirstOrDefault(p => p.Id == passengerId && p.DeleteAt == null);

            if (passenger == null)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, dữ liệu không tồn tại.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            String? id = HttpContext.Session.GetString("AuthId");
            String? role = HttpContext.Session.GetString("Role");

            bool isAdmin = role == "Admin";
            bool isValidSale = role == "Sale" && passenger?.ModifiedById != null && passenger.ModifiedById.ToString() == id;

            if (!(isAdmin || isValidSale))
            {
                TempData["ErrorMessage"] = "Bạn không có quyền xóa hành khách này.";
                return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
            }

            passenger!.DeleteAt = DateTime.Now;
            _context.SaveChanges();

            return Redirect($"{Url.Action("Edit", new { id = passenger?.TourId })}#passenger-list");
        }

        // Countries/Locations handle section

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin")]
        public IActionResult SaveCountry(Country model)
        {
            if (model.Id > 0)
            {
                // Find if exists => update
                var c = _context.Countries.Find(model.Id);
                if (c != null)
                {
                    c.Name = model.Name;
                    c.Code = model.Code;
                }
            }
            else
            {
                // Find if not exists => create
                _context.Countries.Add(model);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAuthorize("Admin")]
        public IActionResult SaveLocation(Location model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            if (model.Id > 0)
            {
                // Find if exists => update
                Location? existing = _context.Locations.FirstOrDefault(l => l.Id == model.Id);
                if (existing != null)
                {
                    existing.LocationName = model.LocationName;
                    _context.SaveChanges();
                }
            }
            else
            {
                // Find if not exists => create
                _context.Locations.Add(model);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
