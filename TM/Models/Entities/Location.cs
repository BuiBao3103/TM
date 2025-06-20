using System;
using System.Collections.Generic;

namespace TM.Models.Entities;

public partial class Location
{
    public int Id { get; set; }

    public string LocationName { get; set; } = null!;

    public int CountryId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Account? ModifiedBy { get; set; }

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
