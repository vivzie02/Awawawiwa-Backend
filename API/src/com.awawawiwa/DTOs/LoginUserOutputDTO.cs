using System;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// LoginUserOutputDTO
    /// </summary>
    public class LoginUserOutputDTO
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}
