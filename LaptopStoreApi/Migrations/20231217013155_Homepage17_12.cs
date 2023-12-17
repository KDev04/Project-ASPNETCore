using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class Homepage17_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homepage",
                columns: table => new
                {
                    HomePageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLaptop = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlideImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaptopMaLaptop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homepage", x => x.HomePageId);
                    table.ForeignKey(
                        name: "FK_Homepage_Laptops_LaptopMaLaptop",
                        column: x => x.LaptopMaLaptop,
                        principalTable: "Laptops",
                        principalColumn: "MaLaptop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homepage_LaptopMaLaptop",
                table: "Homepage",
                column: "LaptopMaLaptop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homepage");
        }
    }
}
