using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOneWithManyRelationshipIntoIngredientUnitEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUnit_Ingredients_IngredientId",
                table: "IngredientUnit");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUnit_Ingredients_IngredientId",
                table: "IngredientUnit",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUnit_Ingredients_IngredientId",
                table: "IngredientUnit");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUnit_Ingredients_IngredientId",
                table: "IngredientUnit",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
