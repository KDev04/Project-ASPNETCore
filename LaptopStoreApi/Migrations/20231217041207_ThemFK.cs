using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class ThemFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homepage_Laptops_LaptopMaLaptop",
                table: "Homepage");

            migrationBuilder.DropIndex(
                name: "IX_Homepage_LaptopMaLaptop",
                table: "Homepage");

            migrationBuilder.DropColumn(
                name: "LaptopMaLaptop",
                table: "Homepage");

            migrationBuilder.CreateIndex(
                name: "IX_Homepage_MaLaptop",
                table: "Homepage",
                column: "MaLaptop");

            migrationBuilder.AddForeignKey(
                name: "FK_Homepage_Laptops_MaLaptop",
                table: "Homepage",
                column: "MaLaptop",
                principalTable: "Laptops",
                principalColumn: "MaLaptop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homepage_Laptops_MaLaptop",
                table: "Homepage");

            migrationBuilder.DropIndex(
                name: "IX_Homepage_MaLaptop",
                table: "Homepage");

            migrationBuilder.AddColumn<int>(
                name: "LaptopMaLaptop",
                table: "Homepage",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homepage_LaptopMaLaptop",
                table: "Homepage",
                column: "LaptopMaLaptop");

            migrationBuilder.AddForeignKey(
                name: "FK_Homepage_Laptops_LaptopMaLaptop",
                table: "Homepage",
                column: "LaptopMaLaptop",
                principalTable: "Laptops",
                principalColumn: "MaLaptop");
        }
    }
}
