namespace TM.Models.Entities;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual Account? ModifiedBy { get; set; }
}
