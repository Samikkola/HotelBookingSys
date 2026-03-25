using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingSys.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfGuestsToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfGuests",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfGuests",
                table: "Reservations");
        }
    }
}
