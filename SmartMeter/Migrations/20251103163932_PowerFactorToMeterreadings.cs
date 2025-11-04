using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMeter.Migrations
{
    /// <inheritdoc />
    public partial class PowerFactorToMeterreadings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "powerfactor",
                table: "meterreading",
                type: "numeric(4,3)",
                precision: 4,
                scale: 3,
                nullable: false,
                defaultValue: 0.85m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "powerfactor",
                table: "meterreading");
        }
    }
}
