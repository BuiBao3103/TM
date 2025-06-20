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
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__3214EC07745155F3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Country__3214EC072B231A63", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Country__Modifie__4F12BBB9",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Location__3214EC070DDE3659", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Location__Countr__5006DFF2",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Location__Modifi__50FB042B",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
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
                    SuggestPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    HhFee = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    DepartureFlightInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ArrivalFlightInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAutoHoldTime = table.Column<bool>(type: "bit", nullable: true),
                    HoldTime = table.Column<int>(type: "int", nullable: true),
                    IsVisaRequired = table.Column<bool>(type: "bit", nullable: true),
                    VisaDeadline = table.Column<DateTime>(type: "datetime", nullable: true),
                    FullPayDeadline = table.Column<DateTime>(type: "datetime", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    DepartureLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartureAssembleTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    RoomNote = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "Available"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tour__3214EC073708CED1", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Tour__LocationId__4B422AD5",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Tour__ModifiedBy__4C364F0E",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
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
                    AssignedPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    CustomerPaid = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    PassportNum = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    DepartureTicket = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ArrivalTicket = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "Reserved"),
                    DepartureFlightInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ArrivalFlightInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Passenge__3214EC072CBEAAAC", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Passenger__Modif__4E1E9780",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Passenger__TourI__4D2A7347",
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
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourSurc__3214EC0738DFF85C", x => x.Id);
                    table.ForeignKey(
                        name: "FK__TourSurch__Modif__52E34C9D",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__TourSurch__TourI__51EF2864",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_ModifiedById",
                table: "Country",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "UQ__Country__737584F6AEA61A1C",
                table: "Country",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Country__A25C5AA7583A9C18",
                table: "Country",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_CountryId",
                table: "Location",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ModifiedById",
                table: "Location",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_ModifiedById",
                table: "Passenger",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_TourId",
                table: "Passenger",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_LocationId",
                table: "Tour",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_ModifiedById",
                table: "Tour",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_TourSurcharge_ModifiedById",
                table: "TourSurcharge",
                column: "ModifiedById");

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

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
