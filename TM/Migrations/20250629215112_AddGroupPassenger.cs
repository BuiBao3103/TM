using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupPassenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PassengerGroupId",
                table: "Passenger",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PassengerGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RepresentativeId = table.Column<int>(type: "int", nullable: true),
                    TotalMember = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PassengerGroup__3214EC07", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PassengerGroup__CreatedBy",
                        column: x => x.CreatedById,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PassengerGroup__ModifiedBy",
                        column: x => x.ModifiedById,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PassengerGroup__Representative",
                        column: x => x.RepresentativeId,
                        principalTable: "Passenger",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__PassengerGroup__Tour",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_PassengerGroupId",
                table: "Passenger",
                column: "PassengerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerGroup_CreatedById",
                table: "PassengerGroup",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerGroup_ModifiedById",
                table: "PassengerGroup",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerGroup_RepresentativeId",
                table: "PassengerGroup",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerGroup_TourId",
                table: "PassengerGroup",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK__Passenger__PassengerGroup",
                table: "Passenger",
                column: "PassengerGroupId",
                principalTable: "PassengerGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Passenger__PassengerGroup",
                table: "Passenger");

            migrationBuilder.DropTable(
                name: "PassengerGroup");

            migrationBuilder.DropIndex(
                name: "IX_Passenger_PassengerGroupId",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "PassengerGroupId",
                table: "Passenger");
        }
    }
}
