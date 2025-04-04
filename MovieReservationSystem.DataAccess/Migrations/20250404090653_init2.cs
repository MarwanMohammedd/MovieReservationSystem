using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReservationSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "MovieSchedules");

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Showtime",
                table: "MovieSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules",
                columns: new[] { "MovieId", "Showtime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Showtime",
                table: "MovieSchedules");

            migrationBuilder.AddColumn<int>(
                name: "StartTime",
                table: "MovieSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules",
                columns: new[] { "MovieId", "StartTime" });
        }
    }
}
