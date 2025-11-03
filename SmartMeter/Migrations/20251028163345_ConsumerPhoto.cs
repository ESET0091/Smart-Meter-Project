using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMeter.Migrations
{
    /// <inheritdoc />
    public partial class ConsumerPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoContentType",
                table: "consumer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "consumer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProfilePhotoSize",
                table: "consumer",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProfilePhotoUpdatedAt",
                table: "consumer",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoUrl",
                table: "consumer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoContentType",
                table: "consumer");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "consumer");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoSize",
                table: "consumer");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoUpdatedAt",
                table: "consumer");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoUrl",
                table: "consumer");
        }
    }
}
