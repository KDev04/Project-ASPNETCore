using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class Changhomepagecolumn : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaptopMaLaptop",
                table: "Homepage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Homepage_LaptopMaLaptop",
                table: "Homepage",
                column: "LaptopMaLaptop");

            migrationBuilder.AddForeignKey(
                name: "FK_Homepage_Laptops_LaptopMaLaptop",
                table: "Homepage",
                column: "LaptopMaLaptop",
                principalTable: "Laptops",
                principalColumn: "MaLaptop",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
