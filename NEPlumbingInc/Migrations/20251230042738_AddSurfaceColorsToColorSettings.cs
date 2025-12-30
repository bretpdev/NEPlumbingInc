using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddSurfaceColorsToColorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DarkInputBgColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#3c3c3c");

            migrationBuilder.AddColumn<string>(
                name: "DarkSurfaceAltColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#2d2d30");

            migrationBuilder.AddColumn<string>(
                name: "DarkSurfaceColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#252526");

            migrationBuilder.AddColumn<string>(
                name: "InputBgColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#ffffff");

            migrationBuilder.AddColumn<string>(
                name: "SurfaceAltColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#f8f9fa");

            migrationBuilder.AddColumn<string>(
                name: "SurfaceColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#ffffff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkInputBgColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkSurfaceAltColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkSurfaceColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "InputBgColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "SurfaceAltColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "SurfaceColor",
                table: "ColorSettings");
        }
    }
}
