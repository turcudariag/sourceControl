using Songs.Common.Entities;
using Songs.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Songs.Common;

namespace Songs.Services
{
    public class UsersService : IUsersService
    {
        private List<User> _users = new List<User>();
        private readonly AuthorizationSettings _authorizationSettings;
        private readonly byte[] _salt;

        public UsersService(IOptions<AuthorizationSettings> appSettings)
        {
            _authorizationSettings = appSettings.Value;

            _salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(_salt);
            }
            _users.Add(new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "User", Username = "test", PasswordHash = HashPassword("test"), UserRole = "User" });
            _users.Add(new User { Id = Guid.NewGuid(), FirstName = "Test2", LastName = "User2", Username = "admin", PasswordHash = HashPassword("admin"), UserRole = "Admin" });
        }

        public string Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.PasswordHash == HashPassword(password));
            return user == null ? null : GenerateJwtToken(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(Guid id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Generate a token that is valid for a pre-defined number of minutes.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authorizationSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("role",user.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _authorizationSettings.Issuer,
                audience: _authorizationSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
