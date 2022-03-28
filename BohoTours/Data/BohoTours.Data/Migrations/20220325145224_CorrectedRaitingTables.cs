using Microsoft.EntityFrameworkCore.Migrations;

namespace BohoTours.Data.Migrations
{
    public partial class CorrectedRaitingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "VacationRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "HotelRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "VacationRatings");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "HotelRatings");
        }
    }
}
