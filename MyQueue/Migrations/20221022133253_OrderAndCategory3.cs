using Microsoft.EntityFrameworkCore.Migrations;

namespace MyQueue.Migrations
{
    public partial class OrderAndCategory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "FoodsOrder",
                columns: table => new
                {
                    FoodsId = table.Column<int>(type: "integer", nullable: false),
                    OrdersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodsOrder", x => new { x.FoodsId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_FoodsOrder_Foods_FoodsId",
                        column: x => x.FoodsId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodsOrder_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodsOrder_OrdersId",
                table: "FoodsOrder",
                column: "OrdersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodsOrder");

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
    }
}
