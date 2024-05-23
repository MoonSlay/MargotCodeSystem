using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class removeFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_HouseOccupants_ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropColumn(
                name: "ResidentId",
                table: "Tbl_HouseOccupants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResidentId",
                table: "Tbl_HouseOccupants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_HouseOccupants_ResidentId",
                table: "Tbl_HouseOccupants",
                column: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id");
        }
    }
}
