using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalaryAndAddPassportExpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "TourSurcharge",
                type: "decimal(38,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SuggestPrice",
                table: "Tour",
                type: "decimal(38,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HhFee",
                table: "Tour",
                type: "decimal(38,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPrice",
                table: "Tour",
                type: "decimal(38,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerPaid",
                table: "Passenger",
                type: "decimal(38,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AssignedPrice",
                table: "Passenger",
                type: "decimal(38,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PassportExpiryDate",
                table: "Passenger",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassportExpiryDate",
                table: "Passenger");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "TourSurcharge",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SuggestPrice",
                table: "Tour",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HhFee",
                table: "Tour",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPrice",
                table: "Tour",
                type: "decimal(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerPaid",
                table: "Passenger",
                type: "decimal(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AssignedPrice",
                table: "Passenger",
                type: "decimal(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,0)",
                oldNullable: true);
        }
    }
}
