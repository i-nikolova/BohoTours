using Microsoft.EntityFrameworkCore.Migrations;

namespace BohoTours.Data.Migrations
{
    public partial class AddedBookingDBSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelBooking_HotelRoomPrices_HotelRoomPriceId",
                table: "HotelBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationBooking_VacationPrices_VacationPriceId",
                table: "VacationBooking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationBooking",
                table: "VacationBooking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelBooking",
                table: "HotelBooking");

            migrationBuilder.RenameTable(
                name: "VacationBooking",
                newName: "VacationBookings");

            migrationBuilder.RenameTable(
                name: "HotelBooking",
                newName: "HotelBookings");

            migrationBuilder.RenameIndex(
                name: "IX_VacationBooking_VacationPriceId",
                table: "VacationBookings",
                newName: "IX_VacationBookings_VacationPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationBooking_IsDeleted",
                table: "VacationBookings",
                newName: "IX_VacationBookings_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBooking_IsDeleted",
                table: "HotelBookings",
                newName: "IX_HotelBookings_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBooking_HotelRoomPriceId",
                table: "HotelBookings",
                newName: "IX_HotelBookings_HotelRoomPriceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationBookings",
                table: "VacationBookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelBookings",
                table: "HotelBookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBookings_HotelRoomPrices_HotelRoomPriceId",
                table: "HotelBookings",
                column: "HotelRoomPriceId",
                principalTable: "HotelRoomPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationBookings_VacationPrices_VacationPriceId",
                table: "VacationBookings",
                column: "VacationPriceId",
                principalTable: "VacationPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelBookings_HotelRoomPrices_HotelRoomPriceId",
                table: "HotelBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationBookings_VacationPrices_VacationPriceId",
                table: "VacationBookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationBookings",
                table: "VacationBookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelBookings",
                table: "HotelBookings");

            migrationBuilder.RenameTable(
                name: "VacationBookings",
                newName: "VacationBooking");

            migrationBuilder.RenameTable(
                name: "HotelBookings",
                newName: "HotelBooking");

            migrationBuilder.RenameIndex(
                name: "IX_VacationBookings_VacationPriceId",
                table: "VacationBooking",
                newName: "IX_VacationBooking_VacationPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationBookings_IsDeleted",
                table: "VacationBooking",
                newName: "IX_VacationBooking_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBookings_IsDeleted",
                table: "HotelBooking",
                newName: "IX_HotelBooking_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_HotelBookings_HotelRoomPriceId",
                table: "HotelBooking",
                newName: "IX_HotelBooking_HotelRoomPriceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationBooking",
                table: "VacationBooking",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelBooking",
                table: "HotelBooking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelBooking_HotelRoomPrices_HotelRoomPriceId",
                table: "HotelBooking",
                column: "HotelRoomPriceId",
                principalTable: "HotelRoomPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationBooking_VacationPrices_VacationPriceId",
                table: "VacationBooking",
                column: "VacationPriceId",
                principalTable: "VacationPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
