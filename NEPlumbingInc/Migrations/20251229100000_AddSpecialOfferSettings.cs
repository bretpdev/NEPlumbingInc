using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialOfferSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialOfferSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    HeadingText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfferHeading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfferDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsletterText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxOffersLimit = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialOfferSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialOfferSettings");
        }
    }
}
