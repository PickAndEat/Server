using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PickAndEat.Migrations {
  /// <inheritdoc />
  public partial class InitialCreate : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "Users",
          columns: table => new {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            EmailAddress = table.Column<string>(type: "text", nullable: false),
            Password = table.Column<string>(type: "text", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_Users", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Dishes",
          columns: table => new {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            ImageFilename = table.Column<string>(type: "text", nullable: false),
            UserId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_Dishes", x => x.Id);
            table.ForeignKey(
                      name: "FK_Dishes_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Dishes_UserId",
          table: "Dishes",
          column: "UserId");

      migrationBuilder.CreateIndex(
          name: "IX_Users_EmailAddress",
          table: "Users",
          column: "EmailAddress",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "Dishes");

      migrationBuilder.DropTable(
          name: "Users");
    }
  }
}
