using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Departure_And_Arrival_Ticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTicket",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "DepartureTicket",
                table: "Passenger");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArrivalTicket",
                table: "Passenger",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureTicket",
                table: "Passenger",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }
    }
}
