using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramendo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInstagramToShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "instagram",
                table: "RamenShop",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "instagram",
                table: "RamenShop");
        }
    }
}
