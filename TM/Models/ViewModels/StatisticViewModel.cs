namespace TM.Models.ViewModels
{
    // StatisticViewModel
    public class StatisticViewModel
    {
        public int TotalTours { get; set; }
        public int TotalPassengers { get; set; }
        public decimal TotalRevenue { get; set; }
        public bool IncludeRevenue { get; set; }

        // Filtering properties
        public string? SelectedCountry { get; set; }
        public string? SelectedLocation { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Statistics data
        public List<TourRevenueByDateDto> RevenueByDate { get; set; } = new();
        public List<TourCountByCountryDto> ToursByCountry { get; set; } = new();
        public List<TourCountByLocationDto> ToursByLocation { get; set; } = new();

        // Additional statistics
        public Dictionary<string, decimal> MonthlyRevenue { get; set; } = new();
        public Dictionary<string, int> QuarterlyTours { get; set; } = new();
    }

    // TourRevenueByDateDto.cs
    public class TourRevenueByDateDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int TourCount { get; set; } // Thêm số lượng tour
    }

    // TourCountByCountryDto.cs
    public class TourCountByCountryDto
    {
        public string CountryName { get; set; }
        public int TourCount { get; set; }
        public decimal Revenue { get; set; } // Thêm doanh thu
        public int PassengerCount { get; set; } // Thêm số hành khách
    }

    // TourCountByLocationDto.cs
    public class TourCountByLocationDto
    {
        public string LocationName { get; set; }
        public string CountryName { get; set; }
        public int TourCount { get; set; }
        public decimal Revenue { get; set; } // Thêm doanh thu
        public int PassengerCount { get; set; } // Thêm số hành khách
    }



}
