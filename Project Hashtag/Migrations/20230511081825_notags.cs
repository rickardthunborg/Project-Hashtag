using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Hashtag.Migrations
{
    public partial class notags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Tags_TagID",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TagID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TagID",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "TagID",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TagID",
                table: "Posts",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Tags_TagID",
                table: "Posts",
                column: "TagID",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
