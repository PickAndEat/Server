using Microsoft.EntityFrameworkCore;

namespace PickAndEat.Models {
  public class UserModel {
    public int Id { get; set; }

    public required string EmailAddress { get; set; }

    public required string Password { get; set; }

    public ICollection<DishModel> Dishes { get; set; } = null!;

    public static void OnModelCreating(ModelBuilder builder) {
      builder
        .Entity<UserModel>()
        .HasIndex(u => u.EmailAddress)
        .IsUnique();
    }
  }
}
