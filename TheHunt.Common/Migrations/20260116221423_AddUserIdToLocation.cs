using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheHunt.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecordedByUser",
                table: "Locations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedByUser",
                table: "Locations");
        }
    }
}
