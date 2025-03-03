using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ForumAPI.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendaces",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendaces", x => new { x.EventId, x.User });
                    table.ForeignKey(
                        name: "FK_Attendaces_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostId = table.Column<int>(type: "integer", nullable: true),
                    ParentCommentId = table.Column<int>(type: "integer", nullable: true),
                    EventId = table.Column<int>(type: "integer", nullable: true),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostDislikes",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostDislikes", x => new { x.PostId, x.User });
                    table.ForeignKey(
                        name: "FK_PostDislikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostFavorites",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFavorites", x => new { x.PostId, x.User });
                    table.ForeignKey(
                        name: "FK_PostFavorites_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => new { x.PostId, x.User });
                    table.ForeignKey(
                        name: "FK_PostLikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "integer", nullable: false),
                    User = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => new { x.CommentId, x.User });
                    table.ForeignKey(
                        name: "FK_CommentLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EventId",
                table: "Comments",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendaces");

            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "PostDislikes");

            migrationBuilder.DropTable(
                name: "PostFavorites");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
