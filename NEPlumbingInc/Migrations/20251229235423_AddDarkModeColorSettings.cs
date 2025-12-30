using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddDarkModeColorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DarkAccentColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#9cdcfe");

            migrationBuilder.AddColumn<string>(
                name: "DarkBgColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#1e1e1e");

            migrationBuilder.AddColumn<string>(
                name: "DarkButtonColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#569cd6");

            migrationBuilder.AddColumn<string>(
                name: "DarkHeroBadgeColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#2d2d30");

            migrationBuilder.AddColumn<string>(
                name: "DarkPrimaryColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#569cd6");

            migrationBuilder.AddColumn<string>(
                name: "DarkSecondaryColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#4f8cc4");

            migrationBuilder.AddColumn<string>(
                name: "DarkTextColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#d4d4d4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkAccentColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkBgColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkButtonColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkHeroBadgeColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkPrimaryColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkSecondaryColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkTextColor",
                table: "ColorSettings");
        }
    }
}
