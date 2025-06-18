namespace TM.Models.Entities;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
