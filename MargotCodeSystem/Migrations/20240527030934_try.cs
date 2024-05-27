using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class @try : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Meds_Tbl_Residents_ResidentId",
                table: "Tbl_Meds");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Pets_Tbl_Residents_ResidentId",
                table: "Tbl_Pets");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Pets_ResidentId",
                table: "Tbl_Pets");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Meds_ResidentId",
                table: "Tbl_Meds");

            migrationBuilder.AddColumn<int>(
                name: "ResidentModelId",
                table: "Tbl_Pets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidentModelId",
                table: "Tbl_Meds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Pets_ResidentModelId",
                table: "Tbl_Pets",
                column: "ResidentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Meds_ResidentModelId",
                table: "Tbl_Meds",
                column: "ResidentModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Meds_Tbl_Residents_ResidentModelId",
                table: "Tbl_Meds",
                column: "ResidentModelId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Pets_Tbl_Residents_ResidentModelId",
                table: "Tbl_Pets",
                column: "ResidentModelId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Meds_Tbl_Residents_ResidentModelId",
                table: "Tbl_Meds");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Pets_Tbl_Residents_ResidentModelId",
                table: "Tbl_Pets");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Pets_ResidentModelId",
                table: "Tbl_Pets");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Meds_ResidentModelId",
                table: "Tbl_Meds");

            migrationBuilder.DropColumn(
                name: "ResidentModelId",
                table: "Tbl_Pets");

            migrationBuilder.DropColumn(
                name: "ResidentModelId",
                table: "Tbl_Meds");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Pets_ResidentId",
                table: "Tbl_Pets",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Meds_ResidentId",
                table: "Tbl_Meds",
                column: "ResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Meds_Tbl_Residents_ResidentId",
                table: "Tbl_Meds",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Pets_Tbl_Residents_ResidentId",
                table: "Tbl_Pets",
                column: "ResidentId",
                principalTable: "Tbl_Residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
