using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddBorderColorsToColorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BorderColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#dee2e6");

            migrationBuilder.AddColumn<string>(
                name: "DarkBorderColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#3e3e42");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorderColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "DarkBorderColor",
                table: "ColorSettings");
        }
    }
}
