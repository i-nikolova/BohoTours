namespace BohoTours.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedRatingsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationPrice_Vacation_VacationId",
                table: "VacationPrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationPrice",
                table: "VacationPrice");

            migrationBuilder.RenameTable(
                name: "VacationPrice",
                newName: "VacationPrices");

            migrationBuilder.RenameIndex(
                name: "IX_VacationPrice_VacationId",
                table: "VacationPrices",
                newName: "IX_VacationPrices_VacationId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationPrice_IsDeleted",
                table: "VacationPrices",
                newName: "IX_VacationPrices_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationPrices",
                table: "VacationPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPrices_Vacation_VacationId",
                table: "VacationPrices",
                column: "VacationId",
                principalTable: "Vacation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationPrices_Vacation_VacationId",
                table: "VacationPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationPrices",
                table: "VacationPrices");

            migrationBuilder.RenameTable(
                name: "VacationPrices",
                newName: "VacationPrice");

            migrationBuilder.RenameIndex(
                name: "IX_VacationPrices_VacationId",
                table: "VacationPrice",
                newName: "IX_VacationPrice_VacationId");

            migrationBuilder.RenameIndex(
                name: "IX_VacationPrices_IsDeleted",
                table: "VacationPrice",
                newName: "IX_VacationPrice_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationPrice",
                table: "VacationPrice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPrice_Vacation_VacationId",
                table: "VacationPrice",
                column: "VacationId",
                principalTable: "Vacation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
