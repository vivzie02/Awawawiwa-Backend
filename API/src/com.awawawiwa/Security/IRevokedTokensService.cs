namespace com.awawawiwa.Security
{
    /// <summary>
    /// IRevokedTokensService
    /// </summary>
    public interface IRevokedTokensService
    {
        /// <summary>
        /// Add a revoked token
        /// </summary>
        /// <param name="jti"></param>
        void RevokeToken(string jti);

        /// <summary>
        /// Check if a token is revoked
        /// </summary>
        /// <param name="jti"></param>
        /// <returns></returns>
        bool IsTokenRevoked(string jti);
    }
}
