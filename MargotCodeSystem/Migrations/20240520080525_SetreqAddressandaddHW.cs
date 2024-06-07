using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class SetreqAddressandaddHW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Tbl_Residents",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Tbl_Residents",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Tbl_Residents");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Tbl_Residents");
        }
    }
}
