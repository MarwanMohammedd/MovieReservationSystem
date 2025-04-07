using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReservationSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class timeeonly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "MovieSchedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "MovieSchedules");
        }
    }
}
