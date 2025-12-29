using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEPlumbingInc.Migrations
{
    /// <inheritdoc />
    public partial class AddColorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[ColorSettings]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ColorSettings] (
        [Id] int NOT NULL IDENTITY(1,1),
        [PrimaryColor] nvarchar(max) NOT NULL,
        [SecondaryColor] nvarchar(max) NOT NULL,
        [AccentColor] nvarchar(max) NOT NULL,
        [TextColor] nvarchar(max) NOT NULL,
        [LightBgColor] nvarchar(max) NOT NULL,
        [HeroBadgeColor] nvarchar(max) NOT NULL CONSTRAINT [DF_ColorSettings_HeroBadgeColor] DEFAULT N'#0056b3',
        [ButtonColor] nvarchar(max) NOT NULL CONSTRAINT [DF_ColorSettings_ButtonColor] DEFAULT N'#0066CC',
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_ColorSettings] PRIMARY KEY ([Id])
    );
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorSettings");
        }
    }
}
