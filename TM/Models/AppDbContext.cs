using Microsoft.EntityFrameworkCore;
using TM.Models.Entities;

namespace TM.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourSurcharge> TourSurcharges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC0707FF62DC");

            entity.ToTable("Country");

            entity.HasIndex(e => e.Name, "UQ__Country__737584F67CFF0565").IsUnique();

            entity.HasIndex(e => e.Code, "UQ__Country__A25C5AA781CE55DD").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC074AC8A40E");

            entity.ToTable("Location");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LocationName).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Location__Countr__30C33EC3");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Passenge__3214EC07A9F840C4");

            entity.ToTable("Passenger");

            entity.HasIndex(e => e.Phone, "UQ__Passenge__5C7E359E2F12F510").IsUnique();

            entity.HasIndex(e => e.IdentityNumber, "UQ__Passenge__6354A73F5778F8EB").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Passenge__A9D105347CDF4DC5").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AssignedPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerPaid).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(255);
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValue("Reserved");

            entity.HasOne(d => d.Tour).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Passenger__TourI__2FCF1A8A");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tour__3214EC0714C1900D");

            entity.ToTable("Tour");

            entity.Property(e => e.ArrivalFlightInfo).HasMaxLength(255);
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DepartureFlightInfo).HasMaxLength(255);
            entity.Property(e => e.DepartureLocation).HasMaxLength(100);
            entity.Property(e => e.DiscountPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FullPayDeadline).HasColumnType("datetime");
            entity.Property(e => e.HhFee).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.RoomNote).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.SuggestPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.VisaDeadline).HasColumnType("datetime");

            entity.HasOne(d => d.Location).WithMany(p => p.Tours)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Tour__LocationId__2EDAF651");
        });

        modelBuilder.Entity<TourSurcharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TourSurc__3214EC0741E14AA3");

            entity.ToTable("TourSurcharge");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Tour).WithMany(p => p.TourSurcharges)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__TourSurch__TourI__31B762FC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
