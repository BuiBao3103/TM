using Microsoft.AspNetCore.Mvc.Rendering;
using TM.Models.Entities;

namespace TM.Models.ViewModels
{
    public class ModifyLocationViewModel
    {
        public List<Location> Locations { get; set; }
        public Location EditLocation { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public string Query { get; set; }
        public SelectList CountrySelectList { get; set; }
    }
}
