using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumAPI.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedBy",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "LikedBy",
                table: "Comments",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }
    }
}
