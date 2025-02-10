using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRestrictPropertyToIngredientFKIntoDessertIngredientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredients_Ingredients_IngredientId",
                table: "DessertIngredients");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_UnitId",
                table: "PurchaseOrderDetails",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredients_Ingredients_IngredientId",
                table: "DessertIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_Units_UnitId",
                table: "PurchaseOrderDetails",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredients_Ingredients_IngredientId",
                table: "DessertIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_Units_UnitId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDetails_UnitId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "PurchaseOrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredients_Ingredients_IngredientId",
                table: "DessertIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
