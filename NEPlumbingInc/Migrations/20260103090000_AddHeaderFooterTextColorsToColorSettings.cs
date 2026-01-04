using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260103090000_AddHeaderFooterTextColorsToColorSettings")]
    public partial class AddHeaderFooterTextColorsToColorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderFooterTextColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#ffffff");

            migrationBuilder.AddColumn<string>(
                name: "DarkHeaderFooterTextColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#ffffff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderFooterTextColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkHeaderFooterTextColor",
                table: "ColorSettings");
        }
    }
}
