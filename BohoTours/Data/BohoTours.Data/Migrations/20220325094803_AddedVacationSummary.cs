﻿namespace BohoTours.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedVacationSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Vacation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Vacation");
        }
    }
}
