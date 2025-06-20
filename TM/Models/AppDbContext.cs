using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TM.Models.Entities;

namespace TM.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourSurcharge> TourSurcharges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07745155F3");

            entity.ToTable("Account");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC072B231A63");

            entity.ToTable("Country");

            entity.HasIndex(e => e.Name, "UQ__Country__737584F6AEA61A1C").IsUnique();

            entity.HasIndex(e => e.Code, "UQ__Country__A25C5AA7583A9C18").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.ModifiedBy).WithMany(p => p.Countries)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK__Country__Modifie__4F12BBB9");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC070DDE3659");

            entity.ToTable("Location");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LocationName).HasMaxLength(50);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Location__Countr__5006DFF2");

            entity.HasOne(d => d.ModifiedBy).WithMany(p => p.Locations)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK__Location__Modifi__50FB042B");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Passenge__3214EC072CBEAAAC");

            entity.ToTable("Passenger");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ArrivalFlightInfo).HasMaxLength(255);
            entity.Property(e => e.ArrivalTicket)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AssignedPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerPaid).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DepartureFlightInfo).HasMaxLength(255);
            entity.Property(e => e.DepartureTicket)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(255);
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.PassportNum)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValue("Reserved");

            entity.HasOne(d => d.ModifiedBy).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK__Passenger__Modif__4E1E9780");

            entity.HasOne(d => d.Tour).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__Passenger__TourI__4D2A7347");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tour__3214EC073708CED1");

            entity.ToTable("Tour");

            entity.Property(e => e.ArrivalFlightInfo).HasMaxLength(255);
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.DepartureAssembleTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureFlightInfo).HasMaxLength(255);
            entity.Property(e => e.DepartureLocation).HasMaxLength(100);
            entity.Property(e => e.DiscountPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FullPayDeadline).HasColumnType("datetime");
            entity.Property(e => e.HhFee).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.RoomNote).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValue("Available");
            entity.Property(e => e.SuggestPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VisaDeadline).HasColumnType("datetime");

            entity.HasOne(d => d.Location).WithMany(p => p.Tours)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Tour__LocationId__4B422AD5");

            entity.HasOne(d => d.ModifiedBy).WithMany(p => p.Tours)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK__Tour__ModifiedBy__4C364F0E");
        });

        modelBuilder.Entity<TourSurcharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TourSurc__3214EC0738DFF85C");

            entity.ToTable("TourSurcharge");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.ModifiedBy).WithMany(p => p.TourSurcharges)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK__TourSurch__Modif__52E34C9D");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourSurcharges)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("FK__TourSurch__TourI__51EF2864");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
