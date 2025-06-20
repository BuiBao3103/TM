using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TM.Models;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Controllers
{
    public class StatisticController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StatisticController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index(string? selectedCountry = null, string? selectedLocation = null,
            DateTime? fromDate = null, DateTime? toDate = null, bool includeRevenue = true)
        {
            // Chuẩn bị dữ liệu cho dropdown
            ViewBag.Countries = GetCountriesSelectList();
            ViewBag.Locations = GetLocationsSelectList();

            // Lưu giá trị filter hiện tại
            ViewBag.SelectedCountry = selectedCountry;
            ViewBag.SelectedLocation = selectedLocation;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.IncludeRevenue = includeRevenue;

            // Áp dụng bộ lọc với Include Navigation Properties 
            var filteredTours = ApplyFilters(_context.Tours
                .Where(t => t.Status == "Completed") // Chỉ lấy tour đã hoàn thành
                .Include(t => t.Location)
                 .ThenInclude(l => l.Country)
                .Include(t => t.Passengers)
                .AsQueryable(), selectedCountry, selectedLocation, fromDate, toDate);

            // Tính toán thống kê
            var totalTours = filteredTours.Count();
            var totalPassengers = filteredTours.SelectMany(t => t.Passengers)
                                               .Where(t => t.Status == "Confirmed")
                                               .Count();

            var totalRevenue = includeRevenue ? filteredTours
                                                    .SelectMany(t => t.Passengers)
                                                    .Where(t => t.Status == "Confirmed")
                                                    .Sum(p => p.CustomerPaid ?? 0)
                                                : 0;

            // Doanh thu theo ngày kết thúc tour (chỉ khi includeRevenue = true)
            var revenueByDate = includeRevenue ?
                filteredTours
                    .GroupBy(t => t.EndDate.Date)
                    .Select(g => new TourRevenueByDateDto
                    {
                        Date = g.Key,
                        Revenue = g.SelectMany(t => t.Passengers).Where(t => t.Status == "Confirmed").Sum(p => p.CustomerPaid ?? 0)
                    })
                    .OrderBy(g => g.Date)
                    .ToList() : new List<TourRevenueByDateDto>();

            // Thống kê tour theo quốc gia
            var toursByCountry = filteredTours
                .Where(t => t.Location != null && t.Location.Country != null)
                .GroupBy(t => t.Location.Country.Name)
                .Select(g => new TourCountByCountryDto
                {
                    CountryName = g.Key,
                    TourCount = g.Count(),
                    PassengerCount = g.SelectMany(t => t.Passengers).Where(t => t.Status == "Confirmed").Count(),
                    Revenue = includeRevenue ? g.SelectMany(t => t.Passengers).Where(t => t.Status == "Confirmed").Sum(p => p.CustomerPaid ?? 0) : 0
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();

            var toursByLocation = filteredTours.Where(t => t.Location != null && t.Location.Country != null)
                                                .GroupBy(t => new { t.Location.LocationName, t.Location.Country.Name })
                                                .Select(g => new TourCountByLocationDto
                                                {
                                                    LocationName = g.Key.LocationName,
                                                    CountryName = g.Key.Name,
                                                    TourCount = g.Count(),
                                                    PassengerCount = g.SelectMany(t => t.Passengers)
                                                                    .Where(p => p.Status == "Confirmed")
                                                                     .Count(),
                                                    Revenue = includeRevenue
                                                                        ? g.SelectMany(t => t.Passengers)
                                                                            .Where(p => p.Status == "Confirmed")
                                                                            .Sum(p => p.CustomerPaid ?? 0)
                                                                         : 0
                                                })
                                                  .OrderByDescending(x => x.Revenue)
                                                   .ToList();

            var model = new StatisticViewModel
            {
                TotalTours = totalTours,
                TotalPassengers = totalPassengers,
                TotalRevenue = totalRevenue,
                RevenueByDate = revenueByDate,
                ToursByCountry = toursByCountry,
                ToursByLocation = toursByLocation,
                IncludeRevenue = includeRevenue
            };

            return View(model);
        }

        private IQueryable<Tour> ApplyFilters(IQueryable<Tour> query, string? selectedCountry,
            string? selectedLocation, DateTime? fromDate, DateTime? toDate)
        {
            // Lọc theo quốc gia
            if (!string.IsNullOrEmpty(selectedCountry))
            {
                query = query.Where(t => t.Location != null &&
                    t.Location.Country != null &&
                    t.Location.Country.Name == selectedCountry);
            }

            // Lọc theo địa điểm
            if (!string.IsNullOrEmpty(selectedLocation))
            {
                query = query.Where(t => t.Location != null &&
                    t.Location.LocationName == selectedLocation);
            }

            // Lọc theo khoảng thời gian (sử dụng ngày kết thúc tour)
            if (fromDate.HasValue)
            {
                query = query.Where(t => t.EndDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                // Thêm 1 ngày để bao gồm cả ngày kết thúc
                var endDate = toDate.Value.AddDays(1);
                query = query.Where(t => t.EndDate < endDate);
            }

            return query;
        }

        private List<SelectListItem> GetCountriesSelectList()
        {
            var countries = _context.Tours
                .Include(t => t.Location)
                    .ThenInclude(l => l.Country)
                .Where(t => t.Location != null && t.Location.Country != null)
                .Select(t => t.Location.Country.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Tất cả quốc gia --" }
            };

            selectList.AddRange(countries.Select(c => new SelectListItem { Value = c, Text = c }));
            return selectList;
        }

        private List<SelectListItem> GetLocationsSelectList()
        {
            var locations = _context.Tours
                .Include(t => t.Location)
                .Where(t => t.Location != null)
                .Select(t => t.Location.LocationName)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Tất cả địa điểm --" }
            };

            selectList.AddRange(locations.Select(l => new SelectListItem { Value = l, Text = l }));
            return selectList;
        }

        // API endpoint để lấy locations theo country (cho AJAX)
        [HttpGet]
        public JsonResult GetLocationsByCountry(string country)
        {
            var locations = string.IsNullOrEmpty(country)
                ? _context.Tours
                    .Include(t => t.Location)
                    .Where(t => t.Location != null)
                    .Select(t => new { Value = t.Location.LocationName, Text = t.Location.LocationName })
                    .Distinct()
                    .OrderBy(l => l.Text)
                    .ToList()
                : _context.Tours
                    .Include(t => t.Location)
                        .ThenInclude(l => l.Country)
                    .Where(t => t.Location != null &&
                        t.Location.Country != null &&
                        t.Location.Country.Name == country)
                    .Select(t => new { Value = t.Location.LocationName, Text = t.Location.LocationName })
                    .Distinct()
                    .OrderBy(l => l.Text)
                    .ToList();

            return Json(locations);
        }
    }
}