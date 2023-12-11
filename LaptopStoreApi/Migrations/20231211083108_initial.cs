using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    NgayGiao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TinhTrangDonHang = table.Column<int>(type: "int", nullable: false),
                    NguoiNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChiGiao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDh);
                });

            migrationBuilder.CreateTable(
                name: "Laptops",
                columns: table => new
                {
                    MaLaptop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLaptop = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiamGia = table.Column<byte>(type: "tinyint", nullable: false),
                    LoaiManHinh = table.Column<double>(type: "float", nullable: false),
                    Mau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamSanXuat = table.Column<int>(type: "int", nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptops", x => x.MaLaptop);
                    table.ForeignKey(
                        name: "FK_Laptops_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaLaptop = table.Column<int>(type: "int", nullable: false),
                    MaDh = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<double>(type: "float", nullable: false),
                    GiamGia = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => new { x.MaDh, x.MaLaptop });
                    table.ForeignKey(
                        name: "FK_DonHangCT_DonHang",
                        column: x => x.MaDh,
                        principalTable: "DonHangs",
                        principalColumn: "MaDh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangCT_Laptop",
                        column: x => x.MaLaptop,
                        principalTable: "Laptops",
                        principalColumn: "MaLaptop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaLaptop",
                table: "ChiTietDonHangs",
                column: "MaLaptop");

            migrationBuilder.CreateIndex(
                name: "IX_Laptops_CategoryId",
                table: "Laptops",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "Laptops");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
