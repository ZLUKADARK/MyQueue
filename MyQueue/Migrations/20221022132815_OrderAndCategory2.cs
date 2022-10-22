using Microsoft.EntityFrameworkCore.Migrations;

namespace MyQueue.Migrations
{
    public partial class OrderAndCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Order_OrderId",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_OrderId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Foods");

            migrationBuilder.AddColumn<int>(
                name: "FoodsId",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_FoodsId",
                table: "Order",
                column: "FoodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Foods_FoodsId",
                table: "Order",
                column: "FoodsId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Foods_FoodsId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_FoodsId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FoodsId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Foods",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_OrderId",
                table: "Foods",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Order_OrderId",
                table: "Foods",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
