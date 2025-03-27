using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMP.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DBContextModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_ChannelProfiles_ChannelProfileId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostFormat_PostFormatId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostStatus_PostStatusId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameIndex(
                name: "IX_Post_PostStatusId",
                table: "Posts",
                newName: "IX_Posts_PostStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_PostFormatId",
                table: "Posts",
                newName: "IX_Posts_PostFormatId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_ChannelProfileId",
                table: "Posts",
                newName: "IX_Posts_ChannelProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ChannelProfiles_ChannelProfileId",
                table: "Posts",
                column: "ChannelProfileId",
                principalTable: "ChannelProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostFormat_PostFormatId",
                table: "Posts",
                column: "PostFormatId",
                principalTable: "PostFormat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostStatus_PostStatusId",
                table: "Posts",
                column: "PostStatusId",
                principalTable: "PostStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ChannelProfiles_ChannelProfileId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostFormat_PostFormatId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostStatus_PostStatusId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostStatusId",
                table: "Post",
                newName: "IX_Post_PostStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostFormatId",
                table: "Post",
                newName: "IX_Post_PostFormatId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ChannelProfileId",
                table: "Post",
                newName: "IX_Post_ChannelProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_ChannelProfiles_ChannelProfileId",
                table: "Post",
                column: "ChannelProfileId",
                principalTable: "ChannelProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostFormat_PostFormatId",
                table: "Post",
                column: "PostFormatId",
                principalTable: "PostFormat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostStatus_PostStatusId",
                table: "Post",
                column: "PostStatusId",
                principalTable: "PostStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
