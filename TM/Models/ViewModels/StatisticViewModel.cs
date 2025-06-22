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
        public List<TourRevenueByDateViewModel> RevenueByDate { get; set; } = new();
        public List<TourCountByCountryViewModel> ToursByCountry { get; set; } = new();
        public List<TourCountByLocationViewModel> ToursByLocation { get; set; } = new();

    }

    public class TourRevenueByDateViewModel
    {
        public DateTime Date { get; set; }
        public decimal? Revenue { get; set; }
        public int? TourCount { get; set; }
        public int? PassengerCount { get; set; }
    }

    public class TourCountByCountryViewModel
    {
        public string CountryName { get; set; }
        public int TourCount { get; set; }
        public decimal Revenue { get; set; } 
        public int PassengerCount { get; set; } 
    }

    public class TourCountByLocationViewModel
    {
        public string LocationName { get; set; }
        public string CountryName { get; set; }
        public int TourCount { get; set; }
        public decimal Revenue { get; set; } 
        public int PassengerCount { get; set; }
    }



}
