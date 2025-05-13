using System;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// UserOutputDTO
    /// </summary>
    public class UserOutputDTO
    {
        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets Rating
        /// </summary>
        public int Rating { get; set; }
    }
}
