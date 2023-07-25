using Microsoft.EntityFrameworkCore;

namespace PickAndEat.Models {
  public class DishModel {
    public int Id { get; set; }

    public required string ImageFilename { get; set; }

    public required int UserId { get; set; }

    public UserModel User { get; set; } = null!;

    public static void OnModelCreating(ModelBuilder builder) {
    }
  }
}
