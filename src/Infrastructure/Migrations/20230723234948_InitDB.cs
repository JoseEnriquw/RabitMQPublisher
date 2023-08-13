using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "Email", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1@gmail.com", "User 1" },
                    { 2, new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2@gmail.com", "User 2" },
                    { 3, new DateTime(2023, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3@gmail.com", "User 3" },
                    { 4, new DateTime(2023, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4@gmail.com", "User 4" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
