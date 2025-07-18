using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace com.awawawiwa.Security
{
    /// <summary>
    /// JwtService
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="config"></param>
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generate a JWT token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GenerateToken(Guid userId)
        {
            var claims = new[]
            {
                new Claim("userId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var jwtKey = Environment.GetEnvironmentVariable("Jwt_Key");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Get the user ID from a JWT token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Guid GetUserIdFromToken(string token)
        {
            // TODO: Implement the logic to extract the user ID from the token
            return Guid.Empty;
        }
    }
}