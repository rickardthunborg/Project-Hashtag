using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Hashtag.Migrations
{
    public partial class Namechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_FollowerId",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Follows",
                newName: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_UserID",
                table: "Follows",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_UserID",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Follows",
                newName: "FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
