using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMeter.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatetimeformat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "lastloginutc",
                table: "User",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "installtsutc",
                table: "meter",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedat",
                table: "consumer",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdat",
                table: "consumer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3) without time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "lastloginutc",
                table: "User",
                type: "timestamp(3) without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "installtsutc",
                table: "meter",
                type: "timestamp(3) without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedat",
                table: "consumer",
                type: "timestamp(3) without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdat",
                table: "consumer",
                type: "timestamp(3) without time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
