using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ramendo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameOptionTablesToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_menu_item_option_values_item_options_item_option_id",
                table: "menu_item_option_values");

            migrationBuilder.DropForeignKey(
                name: "fk_menu_item_option_values_option_values_option_value_id",
                table: "menu_item_option_values");

            migrationBuilder.DropForeignKey(
                name: "fk_option_values_option_types_option_type_id",
                table: "option_values");

            migrationBuilder.DropForeignKey(
                name: "fk_review_options_menu_item_rating_rating_id",
                table: "review_options");

            migrationBuilder.DropPrimaryKey(
                name: "pk_review_options",
                table: "review_options");

            migrationBuilder.DropPrimaryKey(
                name: "pk_option_values",
                table: "option_values");

            migrationBuilder.DropPrimaryKey(
                name: "pk_menu_item_option_values",
                table: "menu_item_option_values");

            migrationBuilder.RenameTable(
                name: "review_options",
                newName: "ReviewOption");

            migrationBuilder.RenameTable(
                name: "option_values",
                newName: "OptionValue");

            migrationBuilder.RenameTable(
                name: "menu_item_option_values",
                newName: "MenuItemOptionValue");

            migrationBuilder.RenameIndex(
                name: "ix_review_options_rating_id",
                table: "ReviewOption",
                newName: "ix_review_option_rating_id");

            migrationBuilder.RenameIndex(
                name: "ix_option_values_option_type_id",
                table: "OptionValue",
                newName: "ix_option_value_option_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_menu_item_option_values_option_value_id",
                table: "MenuItemOptionValue",
                newName: "ix_menu_item_option_value_option_value_id");

            migrationBuilder.RenameIndex(
                name: "ix_menu_item_option_values_item_option_id",
                table: "MenuItemOptionValue",
                newName: "ix_menu_item_option_value_item_option_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_review_option",
                table: "ReviewOption",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_option_value",
                table: "OptionValue",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_menu_item_option_value",
                table: "MenuItemOptionValue",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_menu_item_option_value_item_option_item_option_id",
                table: "MenuItemOptionValue",
                column: "item_option_id",
                principalTable: "ItemOption",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_menu_item_option_value_option_values_option_value_id",
                table: "MenuItemOptionValue",
                column: "option_value_id",
                principalTable: "OptionValue",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_option_value_option_type_option_type_id",
                table: "OptionValue",
                column: "option_type_id",
                principalTable: "OptionType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_review_option_menu_item_rating_rating_id",
                table: "ReviewOption",
                column: "rating_id",
                principalTable: "MenuItemRating",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_menu_item_option_value_item_option_item_option_id",
                table: "MenuItemOptionValue");

            migrationBuilder.DropForeignKey(
                name: "fk_menu_item_option_value_option_values_option_value_id",
                table: "MenuItemOptionValue");

            migrationBuilder.DropForeignKey(
                name: "fk_option_value_option_type_option_type_id",
                table: "OptionValue");

            migrationBuilder.DropForeignKey(
                name: "fk_review_option_menu_item_rating_rating_id",
                table: "ReviewOption");

            migrationBuilder.DropPrimaryKey(
                name: "pk_review_option",
                table: "ReviewOption");

            migrationBuilder.DropPrimaryKey(
                name: "pk_option_value",
                table: "OptionValue");

            migrationBuilder.DropPrimaryKey(
                name: "pk_menu_item_option_value",
                table: "MenuItemOptionValue");

            migrationBuilder.RenameTable(
                name: "ReviewOption",
                newName: "review_options");

            migrationBuilder.RenameTable(
                name: "OptionValue",
                newName: "option_values");

            migrationBuilder.RenameTable(
                name: "MenuItemOptionValue",
                newName: "menu_item_option_values");

            migrationBuilder.RenameIndex(
                name: "ix_review_option_rating_id",
                table: "review_options",
                newName: "ix_review_options_rating_id");

            migrationBuilder.RenameIndex(
                name: "ix_option_value_option_type_id",
                table: "option_values",
                newName: "ix_option_values_option_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_menu_item_option_value_option_value_id",
                table: "menu_item_option_values",
                newName: "ix_menu_item_option_values_option_value_id");

            migrationBuilder.RenameIndex(
                name: "ix_menu_item_option_value_item_option_id",
                table: "menu_item_option_values",
                newName: "ix_menu_item_option_values_item_option_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_review_options",
                table: "review_options",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_option_values",
                table: "option_values",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_menu_item_option_values",
                table: "menu_item_option_values",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_menu_item_option_values_item_options_item_option_id",
                table: "menu_item_option_values",
                column: "item_option_id",
                principalTable: "ItemOption",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_menu_item_option_values_option_values_option_value_id",
                table: "menu_item_option_values",
                column: "option_value_id",
                principalTable: "option_values",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_option_values_option_types_option_type_id",
                table: "option_values",
                column: "option_type_id",
                principalTable: "OptionType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_review_options_menu_item_rating_rating_id",
                table: "review_options",
                column: "rating_id",
                principalTable: "MenuItemRating",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
