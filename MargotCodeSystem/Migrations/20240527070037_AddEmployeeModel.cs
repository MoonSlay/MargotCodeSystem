using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MargotCodeSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeModel : Migration
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

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Tbl_Residents");

            migrationBuilder.DropColumn(
                name: "EmployeeDuration",
                table: "Tbl_Residents");

            migrationBuilder.DropColumn(
                name: "Employer",
                table: "Tbl_Residents");

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

            migrationBuilder.CreateTable(
                name: "Tbl_Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ResidentId = table.Column<int>(type: "int", nullable: false),
                    ResidentModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_Employee_Tbl_Residents_ResidentModelId",
                        column: x => x.ResidentModelId,
                        principalTable: "Tbl_Residents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Pets_ResidentModelId",
                table: "Tbl_Pets",
                column: "ResidentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Meds_ResidentModelId",
                table: "Tbl_Meds",
                column: "ResidentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Employee_ResidentModelId",
                table: "Tbl_Employee",
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

            migrationBuilder.DropTable(
                name: "Tbl_Employee");

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

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Tbl_Residents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeDuration",
                table: "Tbl_Residents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Employer",
                table: "Tbl_Residents",
                type: "nvarchar(max)",
                nullable: true);

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
