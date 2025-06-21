using TM.Models.Entities;

namespace TM.Models.ViewModels
{
    public class TourViewModel
    {
        public List<Country> Countries { get; set; } = new();
        public int? SelectedCountryId { get; set; }

        public List<Location> Locations { get; set; } = new();
        public int? SelectedLocationId { get; set; }

        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<TourWithPassengerStatsViewModel> Tours { get; set; } = new();

        public required PaginationViewModel Pagination { get; set; }
    }
}
