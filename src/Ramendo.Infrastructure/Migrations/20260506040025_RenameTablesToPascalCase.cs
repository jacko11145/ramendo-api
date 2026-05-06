using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramendo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_system_settings",
                table: "system_settings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_tokens",
                table: "refresh_tokens");

            migrationBuilder.RenameTable(
                name: "system_settings",
                newName: "SystemSetting");

            migrationBuilder.RenameTable(
                name: "refresh_tokens",
                newName: "RefreshToken");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_tokens_token",
                table: "RefreshToken",
                newName: "ix_refresh_token_token");

            migrationBuilder.AddPrimaryKey(
                name: "pk_system_setting",
                table: "SystemSetting",
                column: "key");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_token",
                table: "RefreshToken",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_system_setting",
                table: "SystemSetting");

            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_token",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "SystemSetting",
                newName: "system_settings");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "refresh_tokens");

            migrationBuilder.RenameIndex(
                name: "ix_refresh_token_token",
                table: "refresh_tokens",
                newName: "ix_refresh_tokens_token");

            migrationBuilder.AddPrimaryKey(
                name: "pk_system_settings",
                table: "system_settings",
                column: "key");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_tokens",
                table: "refresh_tokens",
                column: "id");
        }
    }
}
