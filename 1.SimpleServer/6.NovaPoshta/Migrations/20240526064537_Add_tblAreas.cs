using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _6.NovaPoshta.Migrations
{
    /// <inheritdoc />
    public partial class Add_tblAreas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAreas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAreas");
        }
    }
}
