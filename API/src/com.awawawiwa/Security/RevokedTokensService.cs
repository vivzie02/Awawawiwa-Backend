using System.Collections.Concurrent;
using System;

namespace com.awawawiwa.Security
{
    /// <summary>
    /// RevokedTokensService
    /// </summary>
    public class RevokedTokensService : IRevokedTokensService
    {
        private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

        /// <summary>
        /// Revoke Token
        /// </summary>
        /// <param name="jti"></param>
        public void RevokeToken(string jti)
        {
            _revokedTokens[jti] = DateTime.UtcNow;
        }

        /// <summary>
        /// Check if a token is revoked
        /// </summary>
        /// <param name="jti"></param>
        /// <returns></returns>
        public bool IsTokenRevoked(string jti)
        {
            return _revokedTokens.ContainsKey(jti);
        }
    }
}
