using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReversiblePropertyToUnitConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReversible",
                table: "UnitConversion",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "UnitConversion",
                columns: new[] { "Id", "ConversionFactor", "FromUnitId", "IsReversible", "ToUnitId" },
                values: new object[,]
                {
                    { -6, 0.033m, 7, true, 3 },
                    { -5, 30m, 3, true, 7 },
                    { -4, 0.001m, 6, true, 1 },
                    { -3, 1000m, 1, true, 6 },
                    { -2, 0.001m, 5, true, 2 },
                    { -1, 1000m, 2, true, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "IsReversible",
                table: "UnitConversion");
        }
    }
}
