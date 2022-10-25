using Microsoft.EntityFrameworkCore.Migrations;

namespace MyQueue.Migrations
{
    public partial class Identitynavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Category_CategoryId",
                table: "Foods");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Foods",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Category_CategoryId",
                table: "Foods",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Category_CategoryId",
                table: "Foods");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Foods",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Category_CategoryId",
                table: "Foods",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
