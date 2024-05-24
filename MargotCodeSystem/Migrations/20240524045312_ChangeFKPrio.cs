using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFKPrio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Dashboard_AspNetUsers_UserId",
                table: "Tbl_Dashboard");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Dashboard_Tbl_Residents_ResidentId",
                table: "Tbl_Dashboard");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_HouseOccupants_ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropColumn(
                name: "ResidentId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.AlterColumn<string>(
                name: "fullName",
                table: "Tbl_Dashboard",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tbl_Dashboard",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResidentId",
                table: "Tbl_Dashboard",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Dashboard_AspNetUsers_UserId",
                table: "Tbl_Dashboard",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Dashboard_Tbl_Residents_ResidentId",
                table: "Tbl_Dashboard",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Dashboard_AspNetUsers_UserId",
                table: "Tbl_Dashboard");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Dashboard_Tbl_Residents_ResidentId",
                table: "Tbl_Dashboard");

            migrationBuilder.AddColumn<int>(
                name: "ResidentId",
                table: "Tbl_HouseOccupants",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fullName",
                table: "Tbl_Dashboard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tbl_Dashboard",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ResidentId",
                table: "Tbl_Dashboard",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_HouseOccupants_ResidentId",
                table: "Tbl_HouseOccupants",
                column: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Dashboard_AspNetUsers_UserId",
                table: "Tbl_Dashboard",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Dashboard_Tbl_Residents_ResidentId",
                table: "Tbl_Dashboard",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_Residents_ResidentId",
                table: "Tbl_HouseOccupants",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id");
        }
    }
}
