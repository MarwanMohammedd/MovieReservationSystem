using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReservationSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Tickets_TicketID",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TicketID",
                table: "Seats");

            migrationBuilder.AlterColumn<int>(
                name: "TicketID",
                table: "Seats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TicketID",
                table: "Seats",
                column: "TicketID",
                unique: true,
                filter: "[TicketID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Tickets_TicketID",
                table: "Seats",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Tickets_TicketID",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TicketID",
                table: "Seats");

            migrationBuilder.AlterColumn<int>(
                name: "TicketID",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TicketID",
                table: "Seats",
                column: "TicketID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Tickets_TicketID",
                table: "Seats",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
