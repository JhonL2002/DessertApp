using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertApp.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDessertDemandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DessertDemand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DessertId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Demand = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DessertDemand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DessertDemand_Desserts_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Desserts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DessertDemand_DessertId",
                table: "DessertDemand",
                column: "DessertId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DessertDemand");
        }
    }
}
