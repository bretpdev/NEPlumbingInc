using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialOfferContentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfferBody",
                table: "SpecialOfferSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfferFinePrint",
                table: "SpecialOfferSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfferTitle",
                table: "SpecialOfferSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferBody",
                table: "SpecialOfferSettings");

            migrationBuilder.DropColumn(
                name: "OfferFinePrint",
                table: "SpecialOfferSettings");

            migrationBuilder.DropColumn(
                name: "OfferTitle",
                table: "SpecialOfferSettings");
        }
    }
}
