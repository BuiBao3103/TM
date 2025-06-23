namespace TM.Models.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Country> CountriesCreated { get; set; } = new List<Country>();
    public virtual ICollection<Country> CountriesModified { get; set; } = new List<Country>();

    public virtual ICollection<Location> LocationsCreated { get; set; } = new List<Location>();
    public virtual ICollection<Location> LocationsModified { get; set; } = new List<Location>();

    public virtual ICollection<Passenger> PassengersCreated { get; set; } = new List<Passenger>();
    public virtual ICollection<Passenger> PassengersModified { get; set; } = new List<Passenger>();

    public virtual ICollection<Tour> ToursCreated { get; set; } = new List<Tour>();
    public virtual ICollection<Tour> ToursModified { get; set; } = new List<Tour>();

    public virtual ICollection<TourSurcharge> TourSurchargesCreated { get; set; } = new List<TourSurcharge>();
    public virtual ICollection<TourSurcharge> TourSurchargesModified { get; set; } = new List<TourSurcharge>();
}
