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

using Microsoft.EntityFrameworkCore;

namespace PickAndEat.Models {
  public class UserModel {
    public int Id { get; set; }

    public required string EmailAddress { get; set; }

    public required string Password { get; set; }

    public ICollection<DishModel> Dishes { get; set; } = null!;

    public ICollection<ShoppingListItemModel> ShoppingListItems { get; set; } = null!;

    public static void OnModelCreating(ModelBuilder builder) {
      builder
        .Entity<UserModel>()
        .HasIndex(u => u.EmailAddress)
        .IsUnique();
    }
  }
}
