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

using Blurhash.ImageSharp;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using GraphQL.AspNet.ServerExtensions.MultipartRequests.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PickAndEat.Extensions;
using PickAndEat.Models;
using PickAndEat.Types.Dish;

namespace PickAndEat.Controllers {
  public class DishController : GraphController {
    private Settings Settings { get; }

    private Database Database { get; }

    public DishController(Settings settings, Database database) {
      Settings = settings;
      Database = database;
    }

    [Authorize]
    [Mutation(typeof(CreateType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> Create([FromGraphQL("image", TypeExpression = "Type!")] FileUpload imageUpload) {
      if (imageUpload == null) {
        return Error("Missing image", "MISSING_IMAGE");
      }

      using var imageStream = await imageUpload.OpenFileAsync();
      using var image = Image.Load<Rgb24>(imageStream);

      image.Mutate(i => i.Resize(100, 100));

      var filename = $"{Guid.NewGuid()}.webp";
      var fullPath = Path.Combine(Settings.BlobStoragePath, filename);
      await image.SaveAsWebpAsync(fullPath);

      var dish = new DishModel {
        ImageFilename = filename,
        UserId = User.GetId(),
        ImageBlurhash = Blurhasher.Encode(image, 4, 4)
      };

      Database.Dishes.Add(dish);
      await Database.SaveChangesAsync();

      return Ok(new CreateType { Id = dish.Id });
    }

    [Authorize]
    [Mutation(typeof(SetNameType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> SetName(int id, [FromGraphQL(TypeExpression = "Type!")] string name) {
      var updateCount = await Database.Dishes
        .Where(d => d.Id == id && d.UserId == User.GetId())
        .ExecuteUpdateAsync(d => d
          .SetProperty(d => d.Name, name)
        );

      return Ok(new SetNameType { Success = updateCount > 0 });
    }

    [Authorize]
    [Mutation(typeof(SetProductsType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> SetProcucts(int id, [FromGraphQL(TypeExpression = "[Type!]!")] List<string> products) {
      var updateCount = await Database.Dishes
        .Where(d => d.Id == id && d.UserId == User.GetId())
        .ExecuteUpdateAsync(d => d
          .SetProperty(d => d.Products, products)
        );

      return Ok(new SetProductsType { Success = updateCount > 0 });
    }

    [Authorize]
    [Mutation(typeof(SetNotesType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> SetNotes(int id, [FromGraphQL(TypeExpression = "Type!")] string notes) {
      var updateCount = await Database.Dishes
        .Where(d => d.Id == id && d.UserId == User.GetId())
        .ExecuteUpdateAsync(d => d
          .SetProperty(d => d.Notes, notes)
        );

      return Ok(new SetNotesType { Success = updateCount > 0 });
    }

    [Authorize]
    [Mutation(typeof(AddToShoppingListType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> AddToShoppingList(int id) {
      var dish = await Database.Dishes
        .Where(d => d.Id == id && d.UserId == User.GetId())
        .Select(d => new {
          Id = d.Id,
          Products = d.Products
        })
        .FirstOrDefaultAsync();

      if (dish == null) {
        return NotFound("Dish not found");
      }

      var shoppingListItem = new ShoppingListItemModel {
        Products = dish.Products,
        UserId = User.GetId(),
        DishId = dish.Id
      };

      Database.ShoppingListItems.Add(shoppingListItem);
      await Database.SaveChangesAsync();

      return Ok(new AddToShoppingListType { ShoppingListItemId = shoppingListItem.Id });
    }

    [Authorize]
    [Query(typeof(IEnumerable<ListType>), TypeExpression = "[Type!]!")]
    public async Task<IGraphActionResult> List() {
      var dishes = await Database.Dishes
        .Where(d => d.UserId == User.GetId())
        .Select(d => new ListType {
          Id = d.Id,
          Name = d.Name,
          Products = d.Products,
          Notes = d.Notes,
          ImageBlurhash = d.ImageBlurhash
        })
        .ToListAsync();

      return Ok(dishes);
    }
  }
}
