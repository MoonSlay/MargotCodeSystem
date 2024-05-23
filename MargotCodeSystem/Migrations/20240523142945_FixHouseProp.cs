using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixHouseProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.AlterColumn<int>(
                name: "ResidentId",
                table: "Tbl_HouseOccupants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.AlterColumn<int>(
                name: "ResidentId",
                table: "Tbl_HouseOccupants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
