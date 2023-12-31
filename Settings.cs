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

using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PickAndEat {
  public class Settings {
    public string ConnectionString { get; }
    public SymmetricSecurityKey JwtKey { get; }

    public string BlobStoragePath { get; }

    public Settings(IConfiguration configuration) {
      var jwtKey = configuration["Jwt:Key"];
      if (jwtKey == null) throw new Exception("JWT key not set");

      var connectionString = configuration.GetConnectionString("Default");
      if (connectionString == null) throw new Exception("Default connection string not set");

      var blobStoragePath = configuration["BlobStorage:Path"];
      if (blobStoragePath == null) throw new Exception("Blob storage path not set");
      Directory.CreateDirectory(blobStoragePath);

      ConnectionString = connectionString;
      JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
      BlobStoragePath = blobStoragePath;
    }
  }
}
