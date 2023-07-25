using GraphQL.AspNet.Configuration;
using GraphQL.AspNet.ServerExtensions.MultipartRequests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PickAndEat;

var builder = WebApplication.CreateBuilder(args);
var settings = new Settings(builder.Configuration);

builder.Services
  .AddSingleton(settings);

builder.Services
  .AddDbContext<Database>();

builder.Services.AddDataProtection()
  .PersistKeysToDbContext<Database>();

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
      ValidIssuer = "PickAndEat",
      ValidAudience = "PickAndEat",
      IssuerSigningKey = settings.JwtKey,
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = false,
      ValidateIssuerSigningKey = true
    };

    // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/415
    options.MapInboundClaims = false;
  });

builder.Services
  .AddAuthorization();

builder.Services.AddGraphQL(options => {
  options.AddMultipartRequestSupport();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
  var database = scope.ServiceProvider.GetRequiredService<Database>();
  database.Database.Migrate();
}

if (app.Environment.IsDevelopment()) {
  app.UseDeveloperExceptionPage();
  app.UseGraphQLPlayground("/");
}

app.UseAuthentication();
app.UseAuthorization();
app.UseGraphQL();

await app.RunAsync();
