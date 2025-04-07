using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReservationSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class timee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "MovieSchedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StartTime",
                table: "MovieSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
