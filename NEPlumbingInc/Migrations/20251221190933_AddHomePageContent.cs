using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddHomePageContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomePageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HeroBadgeText = table.Column<string>(type: "TEXT", nullable: false),
                    HeroTitle = table.Column<string>(type: "TEXT", nullable: false),
                    HeroDescription = table.Column<string>(type: "TEXT", nullable: false),
                    FeatureBadge1 = table.Column<string>(type: "TEXT", nullable: false),
                    FeatureBadge2 = table.Column<string>(type: "TEXT", nullable: false),
                    FeatureBadge3 = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageContents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomePageContents");
        }
    }
}
