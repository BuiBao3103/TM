﻿namespace TM.Models.Entities;

public partial class Passenger
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? IdentityNumber { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? TourId { get; set; }

    public decimal? AssignedPrice { get; set; }

    public decimal? CustomerPaid { get; set; }

    public string? PassportNum { get; set; }

    public string Status { get; set; } = null!;

    public string? DepartureFlightInfo { get; set; }

    public string? ArrivalFlightInfo { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? ModifiedBy { get; set; }

    public virtual Tour? Tour { get; set; }
}
