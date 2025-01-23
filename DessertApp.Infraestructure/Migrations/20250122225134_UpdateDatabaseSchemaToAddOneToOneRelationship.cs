using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchemaToAddOneToOneRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IngredientUnits_IngredientId",
                table: "IngredientUnits");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Ingredients",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientUnits_IngredientId",
                table: "IngredientUnits",
                column: "IngredientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IngredientUnits_IngredientId",
                table: "IngredientUnits");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientUnits_IngredientId",
                table: "IngredientUnits",
                column: "IngredientId");
        }
    }
}
