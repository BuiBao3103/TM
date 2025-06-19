using System;
using System.Collections.Generic;

namespace TM.Models.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    public virtual ICollection<TourSurcharge> TourSurcharges { get; set; } = new List<TourSurcharge>();

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
