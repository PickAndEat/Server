// Pick & Eat Server
// Copyright (C) 2023  Louis Matthijssen
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PickAndEat.Migrations {
  /// <inheritdoc />
  public partial class AddShoppingListItems : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AlterColumn<List<string>>(
          name: "Products",
          table: "Dishes",
          type: "jsonb",
          nullable: false,
          defaultValue: new List<string>(),
          oldClrType: typeof(List<string>),
          oldType: "jsonb",
          oldDefaultValue: new List<string>());

      migrationBuilder.CreateTable(
          name: "ShoppingListItems",
          columns: table => new {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Products = table.Column<List<string>>(type: "jsonb", nullable: false),
            UserId = table.Column<int>(type: "integer", nullable: false),
            DishId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_ShoppingListItems", x => x.Id);
            table.ForeignKey(
                      name: "FK_ShoppingListItems_Dishes_DishId",
                      column: x => x.DishId,
                      principalTable: "Dishes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ShoppingListItems_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_ShoppingListItems_DishId",
          table: "ShoppingListItems",
          column: "DishId");

      migrationBuilder.CreateIndex(
          name: "IX_ShoppingListItems_UserId",
          table: "ShoppingListItems",
          column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "ShoppingListItems");

      migrationBuilder.AlterColumn<List<string>>(
          name: "Products",
          table: "Dishes",
          type: "jsonb",
          nullable: false,
          defaultValue: new List<string>(),
          oldClrType: typeof(List<string>),
          oldType: "jsonb",
          oldDefaultValue: new List<string>());
    }
  }
}
