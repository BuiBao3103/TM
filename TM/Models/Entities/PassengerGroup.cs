namespace TM.Models.Entities;

public class PassengerGroup
{
    public int Id { get; set; }

    public int TourId { get; set; }

    public string GroupName { get; set; } = null!;

    public int? RepresentativeId { get; set; }

    public int? TotalMember { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? CreatedById { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Tour? Tour { get; set; }

    public virtual Passenger? Representative { get; set; }

    public virtual Account? CreatedBy { get; set; }

    public virtual Account? ModifiedBy { get; set; }

    public virtual ICollection<Passenger>? Passengers { get; set; }

}
