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

using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickAndEat.Models;

namespace PickAndEat {
  public class Database : DbContext, IDataProtectionKeyContext {
    private Settings Settings { get; }

    public Database(Settings settings) {
      Settings = settings;
    }

    public required DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public required DbSet<UserModel> Users { get; set; }

    public required DbSet<DishModel> Dishes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
      options.UseNpgsql(Settings.ConnectionString);
      options.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder builder) {
      UserModel.OnModelCreating(builder);
      DishModel.OnModelCreating(builder);
    }
  }
}
