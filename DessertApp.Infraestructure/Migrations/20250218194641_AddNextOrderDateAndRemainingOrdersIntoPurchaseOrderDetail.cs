using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNextOrderDateAndRemainingOrdersIntoPurchaseOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextOrderDate",
                table: "PurchaseOrderDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainingOrders",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextOrderDate",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "RemainingOrders",
                table: "PurchaseOrderDetails");
        }
    }
}
