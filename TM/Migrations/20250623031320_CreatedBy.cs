using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{
    /// <inheritdoc />
    public partial class CreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "TourSurcharge",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById1",
                table: "TourSurcharge",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Tour",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Passenger",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourSurcharge_ModifiedById1",
                table: "TourSurcharge",
                column: "ModifiedById1");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_CreatedById",
                table: "Tour",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_CreatedById",
                table: "Passenger",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CreatedById",
                table: "Location",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CreatedById",
                table: "Country",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_Account_CreatedById",
                table: "Country",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Account_CreatedById",
                table: "Location",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Passenger_Account_CreatedById",
                table: "Passenger",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tour_Account_CreatedById",
                table: "Tour",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourSurcharge_Account_ModifiedById1",
                table: "TourSurcharge",
                column: "ModifiedById1",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_Account_CreatedById",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Account_CreatedById",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Passenger_Account_CreatedById",
                table: "Passenger");

            migrationBuilder.DropForeignKey(
                name: "FK_Tour_Account_CreatedById",
                table: "Tour");

            migrationBuilder.DropForeignKey(
                name: "FK_TourSurcharge_Account_ModifiedById1",
                table: "TourSurcharge");

            migrationBuilder.DropIndex(
                name: "IX_TourSurcharge_ModifiedById1",
                table: "TourSurcharge");

            migrationBuilder.DropIndex(
                name: "IX_Tour_CreatedById",
                table: "Tour");

            migrationBuilder.DropIndex(
                name: "IX_Passenger_CreatedById",
                table: "Passenger");

            migrationBuilder.DropIndex(
                name: "IX_Location_CreatedById",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Country_CreatedById",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TourSurcharge");

            migrationBuilder.DropColumn(
                name: "ModifiedById1",
                table: "TourSurcharge");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Tour");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Country");
        }
    }
}
