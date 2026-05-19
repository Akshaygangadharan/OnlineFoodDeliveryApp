using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderItems.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_OrderItem_Status",
                table: "OrderItems");

            migrationBuilder.AddCheckConstraint(
                name: "CK_OrderItem_Status",
                table: "OrderItems",
                sql: "[status] IN ('Pending', 'Preparing', 'Ready', 'Cancelled')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_OrderItem_Status",
                table: "OrderItems");

            migrationBuilder.AddCheckConstraint(
                name: "CK_OrderItem_Status",
                table: "OrderItems",
                sql: "[status] IN ('cart', 'ordered')");
        }
    }
}
