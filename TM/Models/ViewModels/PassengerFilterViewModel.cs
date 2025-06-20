using TM.Enum;

namespace TM.Models.ViewModels
{
    public class PassengerFilterViewModel
    {
        public string? Keyword { get; set; }
        public string? Status { get; set; }
        public PassengerGroup? PassengerGroup { get; set; }
        public required int TourId { get; set; }
    }
}
