using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitConversionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitConversion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUnitId = table.Column<int>(type: "int", nullable: false),
                    ToUnitId = table.Column<int>(type: "int", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitConversion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitConversion_Units_FromUnitId",
                        column: x => x.FromUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitConversion_Units_ToUnitId",
                        column: x => x.ToUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversion_FromUnitId",
                table: "UnitConversion",
                column: "FromUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversion_ToUnitId",
                table: "UnitConversion",
                column: "ToUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitConversion");
        }
    }
}
