
namespace TM.Models.ViewModels
{
    public class TourSurchargeUpdateViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
    }
}
