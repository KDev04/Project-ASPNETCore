using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class Migrate10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_AspNetUsers_UserId",
                table: "Evaluate");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Laptops_LaptopId",
                table: "Evaluate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluate",
                table: "Evaluate");

            migrationBuilder.RenameTable(
                name: "Evaluate",
                newName: "Evaluates");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluate_UserId",
                table: "Evaluates",
                newName: "IX_Evaluates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluate_LaptopId",
                table: "Evaluates",
                newName: "IX_Evaluates_LaptopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluates",
                table: "Evaluates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluates_AspNetUsers_UserId",
                table: "Evaluates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluates_Laptops_LaptopId",
                table: "Evaluates",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "LaptopId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluates_AspNetUsers_UserId",
                table: "Evaluates");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluates_Laptops_LaptopId",
                table: "Evaluates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluates",
                table: "Evaluates");

            migrationBuilder.RenameTable(
                name: "Evaluates",
                newName: "Evaluate");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluates_UserId",
                table: "Evaluate",
                newName: "IX_Evaluate_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluates_LaptopId",
                table: "Evaluate",
                newName: "IX_Evaluate_LaptopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluate",
                table: "Evaluate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_AspNetUsers_UserId",
                table: "Evaluate",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Laptops_LaptopId",
                table: "Evaluate",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "LaptopId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
