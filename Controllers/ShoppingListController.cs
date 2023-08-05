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

using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PickAndEat.Extensions;
using PickAndEat.Types.ShoppingList;

namespace PickAndEat.Controllers {
  public class ShoppingListController : GraphController {
    private Settings Settings { get; }

    private Database Database { get; }

    public ShoppingListController(Settings settings, Database database) {
      Settings = settings;
      Database = database;
    }

    [Authorize]
    [Query(typeof(IEnumerable<ItemsType>), TypeExpression = "[Type!]!")]
    public async Task<IGraphActionResult> Items() {
      var items = await Database.ShoppingListItems
        .Where(sli => sli.UserId == User.GetId())
        .Select(sli => new ItemsType {
          Id = sli.Id,
          Products = sli.Products,
          DishId = sli.DishId
        })
        .ToListAsync();

      return Ok(items);
    }

    [TypeExtension(typeof(ItemsType), "dish", TypeExpression = "Type!")]
    public async Task<Types.Dish.ListType> RetrieveDish(ItemsType item) {
      var dish = await Database.Dishes.FirstAsync(d => d.Id == item.DishId);

      return new Types.Dish.ListType {
        Id = dish.Id,
        Name = dish.Name,
        Notes = dish.Notes,
        Products = dish.Products
      };
    }
  }
}
