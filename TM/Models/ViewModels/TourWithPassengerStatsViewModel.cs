using TM.Models.Entities;

namespace TM.Models.ViewModels
{
    public class TourWithPassengerStatsViewModel : Tour
    {
        public int TotalCustomers { get; set; }
        public int FullPayCustomers { get; set; }
        public int CustomerNoPassport { get; set; }
        public int ReservedCustomer { get; set; }
        public int DepositedCustomer { get; set; }
        public int CustomerFullPayNotTicket { get; set; }
        public string? LocationName { get; set; }
        public string? CountryName { get; set; }
        public decimal? TotalCustomerPaid { get; set; }
        public decimal? TotalAssignedPrice { get; set; }
        public List<PassengerViewModel> Passengers { get; set; } = new();
        public List<TourSurcharge> TourSurcharges { get; set; } = new();
    }
}
