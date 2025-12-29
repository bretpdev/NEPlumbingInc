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
            // This migration was created with a timestamp that sorts BEFORE the migration
            // that originally created the ColorSettings table. Make it safe for new DBs.
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
ELSE
BEGIN
    IF COL_LENGTH('dbo.ColorSettings', 'HeroBadgeColor') IS NULL
        ALTER TABLE [dbo].[ColorSettings] ADD [HeroBadgeColor] nvarchar(max) NOT NULL CONSTRAINT [DF_ColorSettings_HeroBadgeColor] DEFAULT N'#0056b3';

    IF COL_LENGTH('dbo.ColorSettings', 'ButtonColor') IS NULL
        ALTER TABLE [dbo].[ColorSettings] ADD [ButtonColor] nvarchar(max) NOT NULL CONSTRAINT [DF_ColorSettings_ButtonColor] DEFAULT N'#0066CC';
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Best-effort rollback; only drop columns if the table exists.
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[ColorSettings]', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('dbo.ColorSettings', 'HeroBadgeColor') IS NOT NULL
        ALTER TABLE [dbo].[ColorSettings] DROP COLUMN [HeroBadgeColor];

    IF COL_LENGTH('dbo.ColorSettings', 'ButtonColor') IS NOT NULL
        ALTER TABLE [dbo].[ColorSettings] DROP COLUMN [ButtonColor];
END
");
        }
    }
}
