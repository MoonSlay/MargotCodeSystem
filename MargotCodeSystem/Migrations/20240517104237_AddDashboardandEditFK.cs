using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboardandEditFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Residents_AspNetUsers_UserId",
                table: "Tbl_Residents");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Residents_UserId",
                table: "Tbl_Residents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tbl_Residents");

            migrationBuilder.CreateTable(
                name: "Tbl_Dashboard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    middleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    seniorCitizen = table.Column<bool>(type: "bit", nullable: false),
                    medicationUser = table.Column<bool>(type: "bit", nullable: false),
                    streetSweeper = table.Column<bool>(type: "bit", nullable: false),
                    petOwner = table.Column<bool>(type: "bit", nullable: false),
                    activeResident = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ResidentId = table.Column<int>(type: "int", nullable: false),
                    HouseOccupantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Dashboard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_Dashboard_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tbl_Dashboard_Tbl_HouseOccupants_HouseOccupantId",
                        column: x => x.HouseOccupantId,
                        principalTable: "Tbl_HouseOccupants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tbl_Dashboard_Tbl_Residents_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Tbl_Residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Dashboard_HouseOccupantId",
                table: "Tbl_Dashboard",
                column: "HouseOccupantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Dashboard_ResidentId",
                table: "Tbl_Dashboard",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Dashboard_UserId",
                table: "Tbl_Dashboard",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Dashboard");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tbl_Residents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Residents_UserId",
                table: "Tbl_Residents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Residents_AspNetUsers_UserId",
                table: "Tbl_Residents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
