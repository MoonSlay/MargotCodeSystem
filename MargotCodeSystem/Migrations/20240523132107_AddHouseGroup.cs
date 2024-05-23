using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHouseGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HouseName",
                table: "Tbl_HouseOccupants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HouseOccupantGroupModelId",
                table: "Tbl_HouseOccupants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tbl_HouseGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_HouseGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_HouseOccupants_HouseOccupantGroupModelId",
                table: "Tbl_HouseOccupants",
                column: "HouseOccupantGroupModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_HouseGroup_HouseOccupantGroupModelId",
                table: "Tbl_HouseOccupants",
                column: "HouseOccupantGroupModelId",
                principalTable: "Tbl_HouseGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_HouseOccupants_Tbl_HouseGroup_HouseOccupantGroupModelId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropTable(
                name: "Tbl_HouseGroup");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_HouseOccupants_HouseOccupantGroupModelId",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropColumn(
                name: "HouseName",
                table: "Tbl_HouseOccupants");

            migrationBuilder.DropColumn(
                name: "HouseOccupantGroupModelId",
                table: "Tbl_HouseOccupants");
        }
    }
}
