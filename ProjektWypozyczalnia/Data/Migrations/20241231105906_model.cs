using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektWypozyczalnia.Data.Migrations
{
    /// <inheritdoc />
    public partial class model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_Manufacturer_ManufacturerId",
                table: "Model");

            migrationBuilder.RenameColumn(
                name: "ManufacturerId",
                table: "Model",
                newName: "ManufacturerId1");

            migrationBuilder.RenameIndex(
                name: "IX_Model_ManufacturerId",
                table: "Model",
                newName: "IX_Model_ManufacturerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Manufacturer_ManufacturerId1",
                table: "Model",
                column: "ManufacturerId1",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Model_Manufacturer_ManufacturerId1",
                table: "Model");

            migrationBuilder.RenameColumn(
                name: "ManufacturerId1",
                table: "Model",
                newName: "ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Model_ManufacturerId1",
                table: "Model",
                newName: "IX_Model_ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Manufacturer_ManufacturerId",
                table: "Model",
                column: "ManufacturerId",
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
