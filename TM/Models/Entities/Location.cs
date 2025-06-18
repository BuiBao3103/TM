namespace TM.Models.Entities;

public partial class Location
{
    public int Id { get; set; }

    public string LocationName { get; set; } = null!;

    public int CountryId { get; set; }

    public string? Description { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
