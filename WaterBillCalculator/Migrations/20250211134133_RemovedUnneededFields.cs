using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterBillCalculator.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnneededFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousReading",
                table: "Readings");

            migrationBuilder.DropColumn(
                name: "CurrentReading",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PreviousReading",
                table: "Bills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousReading",
                table: "Readings",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentReading",
                table: "Bills",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousReading",
                table: "Bills",
                type: "decimal(65,30)",
                nullable: true);
        }
    }
}
