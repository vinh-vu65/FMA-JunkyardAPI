using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JunkyardWebApp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Make = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Colour = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CarId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_Parts_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "Colour", "Make", "Model", "Year" },
                values: new object[,]
                {
                    { 1, 1, "Toyota", "Corolla", 2005 },
                    { 2, 2, "Ford", "Falcon", 1995 },
                    { 3, 3, "Honda", "Accord", 2012 },
                    { 4, 5, "Nissan", "Silvia", 2003 }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "PartId", "CarId", "Category", "Description", "Price" },
                values: new object[,]
                {
                    { 1, 1, 0, "Engine for 2005 Corolla", 1500.00m },
                    { 2, 1, 7, "Exhaust for 2005 Corolla", 700.00m },
                    { 3, 1, 6, "Front Passenger Door for 2005 Corolla", 200.00m },
                    { 4, 1, 8, "Taillights for 2005 Corolla", 30.00m },
                    { 5, 1, 4, "Wheels for 2005 Corolla", 450.00m },
                    { 6, 2, 5, "Tyres for 1995 Falcon", 360.00m },
                    { 7, 2, 3, "Brakes for 1995 Falcon", 820.00m },
                    { 8, 2, 1, "Radiator for 1995 Falcon", 120.00m },
                    { 9, 2, 2, "Suspension for 1995 Falcon", 300.00m },
                    { 10, 3, 0, "Engine for 2012 Accord", 1750.00m },
                    { 11, 3, 8, "Brake light for 2012 Accord", 55.00m },
                    { 12, 3, 6, "Rear driver door for 2012 Accord", 190.00m },
                    { 13, 4, 7, "Exhaust for 2003 Silvia", 290.00m },
                    { 14, 4, 3, "Headlight for 2003 Silvia", 370.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CarId",
                table: "Parts",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
