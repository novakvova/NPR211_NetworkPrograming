using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _6.NovaPoshta.Migrations
{
    /// <inheritdoc />
    public partial class Add_tblSettlements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblSettlements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSettlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblSettlements_tblAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "tblAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblSettlements_AreaId",
                table: "tblSettlements",
                column: "AreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSettlements");
        }
    }
}
