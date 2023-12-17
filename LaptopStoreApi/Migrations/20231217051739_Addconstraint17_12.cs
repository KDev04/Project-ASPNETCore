using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class Addconstraint17_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Homepage_MaLaptop",
                table: "Homepage",
                column: "MaLaptop");

            migrationBuilder.AddForeignKey(
                name: "FK_Homepage_Laptops",
                table: "Homepage",
                column: "MaLaptop",
                principalTable: "Laptops",
                principalColumn: "MaLaptop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homepage_Laptops",
                table: "Homepage");

            migrationBuilder.DropIndex(
                name: "IX_Homepage_MaLaptop",
                table: "Homepage");
        }
    }
}
