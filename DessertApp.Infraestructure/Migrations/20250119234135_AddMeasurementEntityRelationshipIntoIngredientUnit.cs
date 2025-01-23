using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeasurementEntityRelationshipIntoIngredientUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredients_MeasurementUnit_UnitId",
                table: "DessertIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUnit_Ingredients_IngredientId",
                table: "IngredientUnit");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUnit_MeasurementUnit_UnitId",
                table: "IngredientUnit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeasurementUnit",
                table: "MeasurementUnit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientUnit",
                table: "IngredientUnit");

            migrationBuilder.RenameTable(
                name: "MeasurementUnit",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "IngredientUnit",
                newName: "IngredientUnits");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientUnit_UnitId",
                table: "IngredientUnits",
                newName: "IX_IngredientUnits_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientUnit_IngredientId",
                table: "IngredientUnits",
                newName: "IX_IngredientUnits_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientUnits",
                table: "IngredientUnits",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Generic Unit" });

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredients_Units_UnitId",
                table: "DessertIngredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUnits_Ingredients_IngredientId",
                table: "IngredientUnits",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUnits_Units_UnitId",
                table: "IngredientUnits",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DessertIngredients_Units_UnitId",
                table: "DessertIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUnits_Ingredients_IngredientId",
                table: "IngredientUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientUnits_Units_UnitId",
                table: "IngredientUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientUnits",
                table: "IngredientUnits");

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "MeasurementUnit");

            migrationBuilder.RenameTable(
                name: "IngredientUnits",
                newName: "IngredientUnit");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientUnits_UnitId",
                table: "IngredientUnit",
                newName: "IX_IngredientUnit_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientUnits_IngredientId",
                table: "IngredientUnit",
                newName: "IX_IngredientUnit_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeasurementUnit",
                table: "MeasurementUnit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientUnit",
                table: "IngredientUnit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DessertIngredients_MeasurementUnit_UnitId",
                table: "DessertIngredients",
                column: "UnitId",
                principalTable: "MeasurementUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUnit_Ingredients_IngredientId",
                table: "IngredientUnit",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientUnit_MeasurementUnit_UnitId",
                table: "IngredientUnit",
                column: "UnitId",
                principalTable: "MeasurementUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
