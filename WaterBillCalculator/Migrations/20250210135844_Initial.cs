using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WaterBillCalculator.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BillDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StandingCharge = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PreviousReading = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CurrentReading = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BilledUnits = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    BilledAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MeterNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeterLocation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MeterName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meters_Meters_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReadingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PreviousReading = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Reading = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MeterId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    CalculatedBillShare = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readings_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readings_Meters_MeterId",
                        column: x => x.MeterId,
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Meters",
                columns: new[] { "Id", "MeterLocation", "MeterName", "MeterNumber", "ParentId" },
                values: new object[,]
                {
                    { 1, "Property", "Main Meter", "0001", null },
                    { 2, "Upper Field", "Peters Upper Field", "0002", 1 },
                    { 3, "Lower Field", "Peters Lower Field", "0003", 1 },
                    { 4, "Our Field", "Our Field", "0004", 1 },
                    { 5, "Houses", "The Houses", "0005", 1 },
                    { 6, "Riverbank Cottage", "Riverbank Cottage", "0006", 5 },
                    { 7, "Riverside Barn", "Riverside Barn", "0007", 5 },
                    { 8, "Waunwen Farm House", "Waunwen Farm House", "0008", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meters_ParentId",
                table: "Meters",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_BillId",
                table: "Readings",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_MeterId",
                table: "Readings",
                column: "MeterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Readings");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Meters");
        }
    }
}
