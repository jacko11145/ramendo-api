using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramendo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewVisitDateAndOptionalComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Review",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateOnly>(
                name: "visit_date",
                table: "Review",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "visit_date",
                table: "Review");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "Review",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
