using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Country__3214EC0707FF62DC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Location__3214EC074AC8A40E", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Location__Countr__30C33EC3",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    SuggestPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    HhFee = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DepartureFlightInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ArrivalFlightInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAutoHoldTime = table.Column<bool>(type: "bit", nullable: true),
                    HoldTime = table.Column<int>(type: "int", nullable: true),
                    IsVisaRequired = table.Column<bool>(type: "bit", nullable: true),
                    VisaDeadline = table.Column<DateTime>(type: "datetime", nullable: true),
                    FullPayDeadline = table.Column<DateTime>(type: "datetime", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    DepartureLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoomNote = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tour__3214EC0714C1900D", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Tour__LocationId__2EDAF651",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    AssignedPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    CustomerPaid = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "Reserved"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Passenge__3214EC07A9F840C4", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Passenger__TourI__2FCF1A8A",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TourSurcharge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourSurc__3214EC0741E14AA3", x => x.Id);
                    table.ForeignKey(
                        name: "FK__TourSurch__TourI__31B762FC",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Country__737584F67CFF0565",
                table: "Country",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Country__A25C5AA781CE55DD",
                table: "Country",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_CountryId",
                table: "Location",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_TourId",
                table: "Passenger",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_LocationId",
                table: "Tour",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TourSurcharge_TourId",
                table: "TourSurcharge",
                column: "TourId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passenger");

            migrationBuilder.DropTable(
                name: "TourSurcharge");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
