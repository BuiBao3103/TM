using TM.Models.Entities;

namespace TM.Models.ViewModels
{
    public class CountryLocationViewModel
    {
        public List<Country> Countries { get; set; } = new();
        public int? SelectedCountryId { get; set; }
        public List<Location> Locations { get; set; } = new();
    }
}