using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialOfferOptionalAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireAddress",
                table: "SpecialOfferSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireAddress",
                table: "SpecialOfferSettings");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Messages");
        }
    }
}
