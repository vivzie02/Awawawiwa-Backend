using System;

namespace com.awawawiwa.Security
{
    public interface IJwtService
    {
        /// <summary>
        /// Generate a JWT token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GenerateToken(Guid userId);

        /// <summary>
        /// Get the user ID from a JWT token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Guid GetUserIdFromToken(string token);
    }
}
