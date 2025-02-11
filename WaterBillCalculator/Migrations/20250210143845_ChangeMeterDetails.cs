using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterBillCalculator.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMeterDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MeterLocation", "MeterName" },
                values: new object[] { "Main Entrance", "Meter By Entrance" });

            migrationBuilder.UpdateData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: 5,
                column: "MeterLocation",
                value: "Just Before it Splits Between the Houses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MeterLocation", "MeterName" },
                values: new object[] { "Property", "Main Meter" });

            migrationBuilder.UpdateData(
                table: "Meters",
                keyColumn: "Id",
                keyValue: 5,
                column: "MeterLocation",
                value: "Houses");
        }
    }
}
