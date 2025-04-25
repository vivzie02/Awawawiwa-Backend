using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IO.Swagger.Security
{
    /// <summary>
    /// class to handle bearer authentication.
    /// </summary>
    public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// scheme name for authentication handler.
        /// </summary>
        public const string SchemeName = "Bearer";

        public BearerAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// verify that require authorization header exists.
        /// </summary>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (authHeader.Scheme != SchemeName)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                var token = authHeader.Parameter;
                var principal = ValidateToken(token);
                if(principal == null)
                {
                    return AuthenticateResult.Fail("Invalid Token");
                }

                var ticket = new AuthenticationTicket(principal, SchemeName);
                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }

        /// <summary>
        /// Validates the JWT token and returns the ClaimsPrincipal if valid.
        /// </summary>
        private ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var jwtKey = Environment.GetEnvironmentVariable("Jwt_Key"); // Secret key for signing

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtKey); // Secret key for signing

                // Token validation parameters
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero, // Optional: to reduce the clock skew
                    ValidIssuer = "awawawiwa-api", // Replace with your issuer
                    ValidAudience = "awawawiwa-users" // Replace with your audience
                };

                // Validate the token and return the claims principal
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal; // Returns a ClaimsPrincipal with the validated claims
            }
            catch (Exception ex)
            {
                return null; // Return null if token validation fails
            }
        }
    }
}
