using AutoMapper;
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

        [RequireAuthorize("Admin")]
        public IActionResult Index(string? selectedCountry = null, string? selectedLocation = null,
            string? selectedSeller = null, DateTime? fromDate = null, DateTime? toDate = null, string tab = "tour")
        {
            var dateRange = SetDefaultDateRange(fromDate, toDate);
            fromDate = dateRange.fromDate;
            toDate = dateRange.toDate;

            SetupViewBag(selectedCountry, selectedLocation, selectedSeller, fromDate, toDate);
            ViewBag.ActiveTab = tab;

            var filteredTours = GetFilteredTours(selectedCountry, selectedLocation, selectedSeller, fromDate, toDate);

            // Tour statistics
            var statistics = CalculateBasicStatistics(filteredTours);
            var dailyStats = GetDailyStatistics(filteredTours, fromDate, toDate);
            var toursByCountry = GetToursByCountry(filteredTours);
            var toursByLocation = GetToursByLocation(filteredTours);
            
            // Employee statistics
            var employeeStatistics = GetEmployeeStatistics(filteredTours);

            var model = new StatisticViewModel
            {
                TotalTours = statistics.totalTours,
                TotalPassengers = statistics.totalPassengers,
                TotalRevenue = statistics.totalRevenue,
                RevenueByDate = dailyStats,
                ToursByCountry = toursByCountry,
                ToursByLocation = toursByLocation,
                EmployeeStatistics = employeeStatistics
            };

            return View(model);
        }

        #region Private Helper Methods

        private (DateTime? fromDate, DateTime? toDate) SetDefaultDateRange(DateTime? fromDate, DateTime? toDate)
        {
            if (!fromDate.HasValue)
                fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (!toDate.HasValue)
                toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                                      DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            return (fromDate, toDate);
        }

        private void SetupViewBag(string? selectedCountry, string? selectedLocation, string? selectedSeller,
            DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.Countries = GetCountriesSelectList();
            ViewBag.Locations = GetLocationsSelectList();
            ViewBag.Sellers = GetSellersSelectList();
            ViewBag.SelectedCountry = selectedCountry;
            ViewBag.SelectedLocation = selectedLocation;
            ViewBag.SelectedSeller = selectedSeller;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
        }

        private IQueryable<Tour> GetFilteredTours(string? selectedCountry, string? selectedLocation,
            string? selectedSeller, DateTime? fromDate, DateTime? toDate)
        {
            var baseQuery = _context.Tours
                .Where(t => t.Status == "Completed")
                .Include(t => t.Location)
                    .ThenInclude(l => l.Country)
                .Include(t => t.Passengers)
                .AsQueryable();

            return ApplyFilters(baseQuery, selectedCountry, selectedLocation, selectedSeller, fromDate, toDate);
        }

        private (int totalTours, int totalPassengers, decimal totalRevenue) CalculateBasicStatistics(
            IQueryable<Tour> filteredTours)
        {
            var totalTours = filteredTours.Count();
            var PaidPassengers = filteredTours
                .SelectMany(t => t.Passengers)
                .Where(p => p.Status == "Paid");

            var totalPassengers = PaidPassengers.Count();
            var totalRevenue = PaidPassengers.Sum(p => p.CustomerPaid ?? 0);

            return (totalTours, totalPassengers, totalRevenue);
        }

        private List<TourRevenueByDateViewModel> GetDailyStatistics(IQueryable<Tour> filteredTours,
            DateTime? fromDate, DateTime? toDate)
        {
            if (!fromDate.HasValue || !toDate.HasValue)
                return new List<TourRevenueByDateViewModel>();

            var rawDailyStats = GetRawDailyStatistics(filteredTours);
            var dateRange = GenerateDateRange(fromDate.Value, toDate.Value);

            return CreateDailyStatisticsList(dateRange, rawDailyStats);
        }

        private Dictionary<DateTime, (int tourCount, int passengerCount, decimal revenue)> GetRawDailyStatistics(IQueryable<Tour> filteredTours)
        {
            return filteredTours
                .GroupBy(t => t.EndDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TourCount = g.Count(),
                    PassengerCount = g.SelectMany(t => t.Passengers)
                                    .Where(p => p.Status == "Paid")
                                    .Count(),
                    Revenue = g.SelectMany(t => t.Passengers)
                             .Where(p => p.Status == "Paid")
                             .Sum(p => p.CustomerPaid ?? 0)
                })
                .ToDictionary(x => x.Date, x => (x.TourCount, x.PassengerCount, x.Revenue));
        }

        private List<DateTime> GenerateDateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, (toDate.Date - fromDate.Date).Days + 1)
                           .Select(offset => fromDate.Date.AddDays(offset))
                           .ToList();
        }

        private List<TourRevenueByDateViewModel> CreateDailyStatisticsList(List<DateTime> dateRange,
            Dictionary<DateTime, (int tourCount, int passengerCount, decimal revenue)> rawDailyStats)
        {
            var today = DateTime.Today;

            return dateRange
                .Select(date => new TourRevenueByDateViewModel
                {
                    Date = date,
                    TourCount = date > today ? null : (rawDailyStats.ContainsKey(date) ? rawDailyStats[date].tourCount : 0),
                    PassengerCount = date > today ? null : (rawDailyStats.ContainsKey(date) ? rawDailyStats[date].passengerCount : 0),
                    Revenue = date > today ? null : (rawDailyStats.ContainsKey(date) ? rawDailyStats[date].revenue : 0)
                })
                .ToList();
        }

        private List<TourRevenueByDateViewModel> GetRevenueByDate(IQueryable<Tour> filteredTours,
            DateTime? fromDate, DateTime? toDate)
        {
            if (!fromDate.HasValue || !toDate.HasValue)
                return new List<TourRevenueByDateViewModel>();

            var rawRevenueByDate = GetRawRevenueByDate(filteredTours);
            var dateRange = GenerateDateRange(fromDate.Value, toDate.Value);

            return CreateRevenueByDateList(dateRange, rawRevenueByDate);
        }

        private Dictionary<DateTime, decimal> GetRawRevenueByDate(IQueryable<Tour> filteredTours)
        {
            return filteredTours
                .GroupBy(t => t.EndDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.SelectMany(t => t.Passengers)
                             .Where(p => p.Status == "Paid")
                             .Sum(p => p.CustomerPaid ?? 0)
                })
                .ToDictionary(x => x.Date, x => x.Revenue);
        }

        private List<TourRevenueByDateViewModel> CreateRevenueByDateList(List<DateTime> dateRange,
            Dictionary<DateTime, decimal> rawRevenueByDate)
        {
            var today = DateTime.Today;

            return dateRange
                .Select(date => new TourRevenueByDateViewModel
                {
                    Date = date,
                    Revenue = date > today ? null : (rawRevenueByDate.ContainsKey(date) ? rawRevenueByDate[date] : 0)
                })
                .ToList();
        }

        private List<TourCountByCountryViewModel> GetToursByCountry(IQueryable<Tour> filteredTours)
        {
            return filteredTours
                .Where(t => t.Location != null && t.Location.Country != null)
                .GroupBy(t => t.Location.Country.Name)
                .Select(g => new TourCountByCountryViewModel
                {
                    CountryName = g.Key,
                    TourCount = g.Count(),
                    PassengerCount = g.SelectMany(t => t.Passengers)
                                    .Where(p => p.Status == "Paid")
                                    .Count(),
                    Revenue = g.SelectMany(t => t.Passengers)
                             .Where(p => p.Status == "Paid")
                             .Sum(p => p.CustomerPaid ?? 0)
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();
        }

        private List<TourCountByLocationViewModel> GetToursByLocation(IQueryable<Tour> filteredTours)
        {
            return filteredTours
                .Where(t => t.Location != null && t.Location.Country != null)
                .GroupBy(t => new { t.Location.LocationName, t.Location.Country.Name })
                .Select(g => new TourCountByLocationViewModel
                {
                    LocationName = g.Key.LocationName,
                    CountryName = g.Key.Name,
                    TourCount = g.Count(),
                    PassengerCount = g.SelectMany(t => t.Passengers)
                                    .Where(p => p.Status == "Paid")
                                    .Count(),
                    Revenue = g.SelectMany(t => t.Passengers)
                             .Where(p => p.Status == "Paid")
                             .Sum(p => p.CustomerPaid ?? 0)
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();
        }

        private IQueryable<Tour> ApplyFilters(IQueryable<Tour> query, string? selectedCountry,
            string? selectedLocation, string? selectedSeller, DateTime? fromDate, DateTime? toDate)
        {
            if (!string.IsNullOrEmpty(selectedCountry))
            {
                query = query.Where(t => t.Location != null &&
                    t.Location.Country != null &&
                    t.Location.Country.Name == selectedCountry);
            }

            if (!string.IsNullOrEmpty(selectedLocation))
            {
                query = query.Where(t => t.Location != null &&
                    t.Location.LocationName == selectedLocation);
            }

            if (!string.IsNullOrEmpty(selectedSeller) && int.TryParse(selectedSeller, out int sellerId))
            {
                query = query.Where(t => t.CreatedById == sellerId);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.EndDate >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.Date.AddDays(1);
                query = query.Where(t => t.EndDate < endDate);
            }

            return query;
        }

        private List<SelectListItem> GetCountriesSelectList()
        {
            var countries = _context.Countries
                .OrderBy(c => c.Name)
                .Select(c => c.Name)
                .ToList();

            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Tất cả quốc gia" }
            };

            selectList.AddRange(countries.Select(c => new SelectListItem { Value = c, Text = c }));
            return selectList;
        }

        private List<SelectListItem> GetLocationsSelectList()
        {
            var locations = _context.Locations
                .OrderBy(l => l.LocationName)
                .Select(l => l.LocationName)
                .ToList();

            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Tất cả địa điểm" }
            };

            selectList.AddRange(locations.Select(l => new SelectListItem { Value = l, Text = l }));
            return selectList;
        }

        private List<SelectListItem> GetSellersSelectList()
        {
            // Cách tối ưu hơn với join trực tiếp
            var sellers = _context.Tours
                .Where(t => t.CreatedById.HasValue)
                .Join(_context.Accounts,
                      t => t.CreatedById,
                      u => u.Id,
                      (t, u) => new { t.CreatedById, User = u })
                .GroupBy(x => new { x.CreatedById })
                .Select(g => new
                {
                    Id = g.Key.CreatedById.Value,
                    Name = g.FirstOrDefault().User.Username
                })
                .OrderBy(s => s.Name)
                .ToList();

            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Tất cả nhân viên" }
            };

            selectList.AddRange(sellers.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }));

            return selectList;
        }

        private List<EmployeeStatisticItemViewModel> GetEmployeeStatistics(IQueryable<Tour> filteredTours)
        {
            var employeeStats = filteredTours
                .SelectMany(t => t.Passengers)
                .Where(p => p.CreatedById != null && p.CreatedBy != null)
                .GroupBy(p => p.CreatedBy)
                .Select(g => new EmployeeStatisticItemViewModel
                {
                    EmployeeName = g.Key.Username,
                    TotalRevenue = g.Sum(p => p.CustomerPaid ?? 0),
                    TotalCommission = g.Sum(p => p.HhFee),
                    TotalDiscount = g.Sum(p => p.DiscountPrice),
                })
                .ToList();

            employeeStats.ForEach(e => e.TotalEarnings = e.TotalCommission - e.TotalDiscount);

            return employeeStats.OrderByDescending(e => e.TotalEarnings).ToList();
        }

        #endregion

        [HttpGet]
        [RequireAuthorize("Admin", "Sale")]
        public JsonResult GetLocationsByCountry(string country)
        {
            var locations = GetLocationsByCountryQuery(country);
            return Json(locations);
        }

        private List<object> GetLocationsByCountryQuery(string country)
        {
            return string.IsNullOrEmpty(country)
                ? GetAllLocations()
                : GetLocationsBySpecificCountry(country);
        }

        private List<object> GetAllLocations()
        {
            return _context.Locations
                .OrderBy(l => l.LocationName)
                .Select(l => new { Value = l.LocationName, Text = l.LocationName })
                .ToList<object>();
        }

        private List<object> GetLocationsBySpecificCountry(string country)
        {
            return _context.Locations
                .Include(l => l.Country)
                .Where(l => l.Country != null && l.Country.Name == country)
                .OrderBy(l => l.LocationName)
                .Select(l => new { Value = l.LocationName, Text = l.LocationName })
                .ToList<object>();
        }
    }
}