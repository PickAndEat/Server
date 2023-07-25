using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using PickAndEat.Models;

namespace PickAndEat {
  public class Database : DbContext {
    private Settings Settings { get; }

    public Database(Settings settings) {
      Settings = settings;
    }

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
