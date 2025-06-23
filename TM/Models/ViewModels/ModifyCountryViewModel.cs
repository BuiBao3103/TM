using Microsoft.AspNetCore.Mvc;
using TM.Models.Entities;

namespace TM.Models.ViewModels
{
    public class ModifyCountryViewModel
    {
        public List<Country> Countries { get; set; }
        public Country EditCountry { get; set; }
        public PaginationViewModel Pagination { get; set; }
        public string Query { get; set; }
    }
}
