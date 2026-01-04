using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddHeaderFooterBackgroundColorsToColorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DarkHeaderFooterBgColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#2d2d30");

            migrationBuilder.AddColumn<string>(
                name: "HeaderFooterBgColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#0066CC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkHeaderFooterBgColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "HeaderFooterBgColor",
                table: "ColorSettings");
        }
    }
}
