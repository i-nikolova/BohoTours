namespace BohoTours.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedVacationDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TownVacation_Vacation_VacationsId",
                table: "TownVacation");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacation_Countries_CountryId",
                table: "Vacation");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacation_Transport_TransportId",
                table: "Vacation");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationImages_Vacation_VacationId",
                table: "VacationImages");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationPrices_Vacation_VacationId",
                table: "VacationPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationRatings_Vacation_VacationId",
                table: "VacationRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vacation",
                table: "Vacation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transport",
                table: "Transport");

            migrationBuilder.RenameTable(
                name: "Vacation",
                newName: "Vacations");

            migrationBuilder.RenameTable(
                name: "Transport",
                newName: "Transports");

            migrationBuilder.RenameIndex(
                name: "IX_Vacation_TransportId",
                table: "Vacations",
                newName: "IX_Vacations_TransportId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacation_IsDeleted",
                table: "Vacations",
                newName: "IX_Vacations_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Vacation_CountryId",
                table: "Vacations",
                newName: "IX_Vacations_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Transport_IsDeleted",
                table: "Transports",
                newName: "IX_Transports_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vacations",
                table: "Vacations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transports",
                table: "Transports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TownVacation_Vacations_VacationsId",
                table: "TownVacation",
                column: "VacationsId",
                principalTable: "Vacations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationImages_Vacations_VacationId",
                table: "VacationImages",
                column: "VacationId",
                principalTable: "Vacations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPrices_Vacations_VacationId",
                table: "VacationPrices",
                column: "VacationId",
                principalTable: "Vacations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRatings_Vacations_VacationId",
                table: "VacationRatings",
                column: "VacationId",
                principalTable: "Vacations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Countries_CountryId",
                table: "Vacations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Transports_TransportId",
                table: "Vacations",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TownVacation_Vacations_VacationsId",
                table: "TownVacation");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationImages_Vacations_VacationId",
                table: "VacationImages");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationPrices_Vacations_VacationId",
                table: "VacationPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationRatings_Vacations_VacationId",
                table: "VacationRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Countries_CountryId",
                table: "Vacations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Transports_TransportId",
                table: "Vacations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vacations",
                table: "Vacations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transports",
                table: "Transports");

            migrationBuilder.RenameTable(
                name: "Vacations",
                newName: "Vacation");

            migrationBuilder.RenameTable(
                name: "Transports",
                newName: "Transport");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_TransportId",
                table: "Vacation",
                newName: "IX_Vacation_TransportId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_IsDeleted",
                table: "Vacation",
                newName: "IX_Vacation_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_CountryId",
                table: "Vacation",
                newName: "IX_Vacation_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Transports_IsDeleted",
                table: "Transport",
                newName: "IX_Transport_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vacation",
                table: "Vacation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transport",
                table: "Transport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TownVacation_Vacation_VacationsId",
                table: "TownVacation",
                column: "VacationsId",
                principalTable: "Vacation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacation_Countries_CountryId",
                table: "Vacation",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacation_Transport_TransportId",
                table: "Vacation",
                column: "TransportId",
                principalTable: "Transport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationImages_Vacation_VacationId",
                table: "VacationImages",
                column: "VacationId",
                principalTable: "Vacation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationPrices_Vacation_VacationId",
                table: "VacationPrices",
                column: "VacationId",
                principalTable: "Vacation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationRatings_Vacation_VacationId",
                table: "VacationRatings",
                column: "VacationId",
                principalTable: "Vacation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
