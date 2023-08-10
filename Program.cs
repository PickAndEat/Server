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

using GraphQL.AspNet.Configuration;
using GraphQL.AspNet.ServerExtensions.MultipartRequests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PickAndEat;

var builder = WebApplication.CreateBuilder(args);
var settings = new Settings(builder.Configuration);

builder.Services
  .AddSingleton(settings)
  .AddDbContext<Database>();

builder.Services
  .AddDataProtection()
  .PersistKeysToDbContext<Database>()
  .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration {
    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
  });

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

builder.Services
  .AddGraphQL(options => {
    options.AddMultipartRequestSupport();
  });

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
  var database = scope.ServiceProvider.GetRequiredService<Database>();
  database.Database.Migrate();
}

if (app.Environment.IsDevelopment()) {
  app.UseDeveloperExceptionPage();
}

app.UseGraphQLPlayground("/graphql/playground");

app.UseAuthentication();
app.UseAuthorization();
app.UseGraphQL();

await app.RunAsync();
