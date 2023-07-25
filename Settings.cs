using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PickAndEat {
  public class Settings {
    public string ConnectionString { get; }
    public SymmetricSecurityKey JwtKey { get; }

    public Settings(IConfiguration configuration) {
      var jwtKey = configuration["Jwt:Key"];
      if (jwtKey == null) throw new Exception("JWT key not set");

      var connectionString = configuration.GetConnectionString("Default");
      if (connectionString == null) throw new Exception("Default connection string not set");

      ConnectionString = connectionString;
      JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    }
  }
}
