using System.Collections.Concurrent;
using System;
using System.Threading;

namespace com.awawawiwa.Security
{
    /// <summary>
    /// RevokedTokensService
    /// </summary>
    public class RevokedTokensService : IRevokedTokensService, IDisposable
    {
        private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// Call cleanup every day to remove expired tokens
        /// </summary>
        public RevokedTokensService()
        {
            _cleanupTimer = new Timer(CleanupRevokedTokens, null, TimeSpan.FromDays(1), TimeSpan.FromDays(1));
        }

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

        /// <summary>
        /// Dispose
        /// </summary>

        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }

        // clean up revoked tokens older than 1 days
        private static void CleanupRevokedTokens(object state)
        {
            var expiration = DateTime.UtcNow.AddDays(-1);
            foreach (var token in _revokedTokens)
            {
                if (token.Value < expiration)
                {
                    _revokedTokens.TryRemove(token.Key, out _);
                }
            }
        }
    }
}
