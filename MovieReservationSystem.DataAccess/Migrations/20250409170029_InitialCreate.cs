using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReservationSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "MovieSchedules",
                newName: "Showtime");

            migrationBuilder.RenameIndex(
                name: "IX_MovieSchedules_MovieId_StartTime",
                table: "MovieSchedules",
                newName: "IX_MovieSchedules_MovieId_Showtime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Showtime",
                table: "MovieSchedules",
                newName: "StartTime");

            migrationBuilder.RenameIndex(
                name: "IX_MovieSchedules_MovieId_Showtime",
                table: "MovieSchedules",
                newName: "IX_MovieSchedules_MovieId_StartTime");
        }
    }
}
