using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class Migrate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Carts_LaptopId",
                table: "Carts",
                column: "LaptopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Laptops_LaptopId",
                table: "Carts",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "LaptopId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Laptops_LaptopId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_LaptopId",
                table: "Carts");
        }
    }
}
