using System;
using System.Collections.Generic;

namespace TM.Models.Entities;

public partial class Tour
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

    public bool? IsAutoHoldTime { get; set; }

    public int? HoldTime { get; set; }

    public bool? IsVisaRequired { get; set; }

    public DateTime? VisaDeadline { get; set; }

    public DateTime? FullPayDeadline { get; set; }

    public int? LocationId { get; set; }

    public string? DepartureLocation { get; set; }

    public string? RoomNote { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    public virtual ICollection<TourSurcharge> TourSurcharges { get; set; } = new List<TourSurcharge>();
}
