using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EntityFramework.Exceptions.Common;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PickAndEat.Models;
using PickAndEat.Types.User;

namespace PickAndEat.Controllers {
  public class UserController : GraphController {
    private Database Database { get; }
    private Settings Settings { get; }

    public UserController(Database database, Settings settings) {
      Database = database;
      Settings = settings;
    }

    [Mutation(typeof(RegisterType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> Register([FromGraphQL(TypeExpression = "Type!")] string emailAddress, [FromGraphQL(TypeExpression = "Type!")] string password) {
      var passwordHasher = new PasswordHasher<UserModel>();

      var user = new UserModel { EmailAddress = emailAddress, Password = string.Empty };
      user.Password = passwordHasher.HashPassword(user, password);

      Database.Users.Add(user);

      try {
        await Database.SaveChangesAsync();
      } catch (UniqueConstraintException exception) {
        return Error("User already exists", "USER_ALREADY_EXISTS", exception);
      }

      return Ok(new RegisterType { Id = user.Id });
    }

    [Mutation(typeof(LoginType), TypeExpression = "Type!")]
    public async Task<IGraphActionResult> Login([FromGraphQL(TypeExpression = "Type!")] string emailAddress, [FromGraphQL(TypeExpression = "Type!")] string password) {
      var user = await Database.Users.Where(u => u.EmailAddress == emailAddress).FirstOrDefaultAsync();

      if (user == null) {
        return Error("Invalid username", "INVALID_CREDENTIALS");
      }

      var passwordHasher = new PasswordHasher<UserModel>();
      var passwordStatus = passwordHasher.VerifyHashedPassword(user, user.Password, password);

      if (passwordStatus == PasswordVerificationResult.Failed) {
        return Error("Invalid password", "INVALID_CREDENTIALS");
      } else if (passwordStatus == PasswordVerificationResult.SuccessRehashNeeded) {
        user.Password = passwordHasher.HashPassword(user, password);
        await Database.SaveChangesAsync();
      }

      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, $"{user.Id}")
      };

      var signingCredentials = new SigningCredentials(Settings.JwtKey, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken("PickAndEat", "PickAndEat", claims, signingCredentials: signingCredentials);

      var handler = new JwtSecurityTokenHandler();
      return Ok(new LoginType { AccessToken = handler.WriteToken(token) });
    }
  }
}
