using System;
using System.Collections.Generic;

namespace TM.Models.Entities;

public partial class TourSurcharge
{
    public int Id { get; set; }

    public int? TourId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Tour? Tour { get; set; }
}
