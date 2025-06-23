using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{
    /// <inheritdoc />
    public partial class CreatedByAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourSurcharge_Account_ModifiedById1",
                table: "TourSurcharge");

            migrationBuilder.DropIndex(
                name: "IX_TourSurcharge_ModifiedById1",
                table: "TourSurcharge");

            migrationBuilder.DropColumn(
                name: "ModifiedById1",
                table: "TourSurcharge");

            migrationBuilder.CreateIndex(
                name: "IX_TourSurcharge_CreatedById",
                table: "TourSurcharge",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TourSurcharge_Account_CreatedById",
                table: "TourSurcharge",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourSurcharge_Account_CreatedById",
                table: "TourSurcharge");

            migrationBuilder.DropIndex(
                name: "IX_TourSurcharge_CreatedById",
                table: "TourSurcharge");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById1",
                table: "TourSurcharge",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourSurcharge_ModifiedById1",
                table: "TourSurcharge",
                column: "ModifiedById1");

            migrationBuilder.AddForeignKey(
                name: "FK_TourSurcharge_Account_ModifiedById1",
                table: "TourSurcharge",
                column: "ModifiedById1",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
