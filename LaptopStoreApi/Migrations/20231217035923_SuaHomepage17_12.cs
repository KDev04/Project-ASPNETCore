using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class SuaHomepage17_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homepage_Laptops_MaLaptop",
                table: "Homepage");

            migrationBuilder.AlterColumn<int>(
                name: "LaptMaLaptop",
                table: "Homepage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Homepage_Laptops_LaptopMaLaptop",
                table: "Homepage",
                column: "LaptopMaLaptop",
                principalTable: "Laptops",
                principalColumn: "MaLaptop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homepage_Laptops_LaptopMaLaptop",
                table: "Homepage");

            migrationBuilder.AlterColumn<int>(
                name: "LaptopMaLaptop",
                table: "Homepage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
