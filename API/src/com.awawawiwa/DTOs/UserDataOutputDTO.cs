using System;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// UserDataOutputDTO
    /// </summary>
    public class UserDataOutputDTO
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Rating
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// ProfilePicture
        /// </summary>
        public string ProfilePicture { get; set; }
    }
}
