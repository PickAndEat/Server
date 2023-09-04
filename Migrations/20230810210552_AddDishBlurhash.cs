﻿// Pick & Eat Server
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

#nullable disable

namespace PickAndEat.Migrations {
  /// <inheritdoc />
  public partial class AddDishBlurhash : Migration {
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

      migrationBuilder.AddColumn<string>(
          name: "ImageBlurhash",
          table: "Dishes",
          type: "text",
          nullable: false,
          defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropColumn(
          name: "ImageBlurhash",
          table: "Dishes");

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