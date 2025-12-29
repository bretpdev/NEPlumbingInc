using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddHeroBadgeAndButtonColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeroBadgeColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#0056b3");

            migrationBuilder.AddColumn<string>(
                name: "ButtonColor",
                table: "ColorSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "#0066CC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeroBadgeColor",
                table: "ColorSettings");

            migrationBuilder.DropColumn(
                name: "ButtonColor",
                table: "ColorSettings");
        }
    }
}
