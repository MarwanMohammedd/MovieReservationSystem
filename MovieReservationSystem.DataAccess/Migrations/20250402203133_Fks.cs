using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReservationSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MovieSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSchedules_MovieId",
                table: "MovieSchedules",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules");

            migrationBuilder.DropIndex(
                name: "IX_MovieSchedules_MovieId",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MovieSchedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieSchedules",
                table: "MovieSchedules",
                columns: new[] { "MovieId", "StartTime" });
        }
    }
}
