using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edu_DB_ASP.Migrations
{
    public partial class UpdateSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Badges' AND xtype='U')
                BEGIN
                    CREATE TABLE [Badges] (
                        [BadgeID] int NOT NULL IDENTITY,
                        [BadgeTitle] varchar(255) NOT NULL,
                        [BadgeDescription] varchar(500) NULL,
                        [CriteriaToUnlock] varchar(500) NULL,
                        [PointsValue] decimal(5,2) NULL,
                        CONSTRAINT [PK__Badges__1918237CBBA0B7BA] PRIMARY KEY ([BadgeID])
                    );
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('Badges', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE [Badges];
                END
            ");
        }
    }
}
