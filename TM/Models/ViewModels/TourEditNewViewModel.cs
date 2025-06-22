using System.ComponentModel.DataAnnotations;
using TM.Models.Entities;

namespace TM.Models.ViewModels
{
    public class TourEditNewViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public decimal SuggestPrice { get; set; }

        public decimal? DiscountPrice { get; set; }

        public decimal HhFee { get; set; }

        public string? DepartureFlightInfo { get; set; }

        public string? ArrivalFlightInfo { get; set; }

        [Required(ErrorMessage = "Mã chuyến bay đi không được để trống")]
        public string? ArrivalFlightCode { get; set; }
        
        [Required(ErrorMessage = "Mã chuyến bay đi không được để trống")]
        public string? DepartureFlightCode { get; set; }
        public bool? IsAutoHoldTime { get; set; }

        public int? HoldTime { get; set; }

        public bool? IsVisaRequired { get; set; }

        public DateTime? VisaDeadline { get; set; }

        public DateTime? FullPayDeadline { get; set; }

        public int? LocationId { get; set; }

        public string? DepartureLocation { get; set; }

        public string? ArrivalLocation { get; set; }

        public string? DepartureLocationCode { get; set; }
        public string? ArrivalLocationCode { get; set; }

        public string? DepartureTime { get; set; }
        public string? ArrivalTime { get; set; }
        public DateTime? DepartureAssembleTime { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? RoomNote { get; set; }

        public string? Note { get; set; }

        public string Status { get; set; } = null!;

    }
}
