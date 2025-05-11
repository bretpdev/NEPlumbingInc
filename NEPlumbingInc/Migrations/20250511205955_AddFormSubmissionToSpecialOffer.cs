using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddFormSubmissionToSpecialOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SpecialOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FormSubmitted",
                table: "SpecialOffers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FormSubmittedAt",
                table: "SpecialOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "SpecialOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SpecialOffers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "SpecialOffers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "FormSubmitted",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "FormSubmittedAt",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "SpecialOffers");
        }
    }
}
