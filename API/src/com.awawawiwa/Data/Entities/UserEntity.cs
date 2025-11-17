using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.awawawiwa.Data.Entities
{
    /// <summary>
    /// UserEntity
    /// </summary>
    [Table("users")]
    public class UserEntity
    {
        /// <summary>
        /// user id
        /// </summary>
        [Key]
        [Column("userId")]
        public Guid UserId { get; set; }
        /// <summary>
        /// username 
        /// </summary>
        [Column("username")]
        [Required(ErrorMessage = "Username may not be empty")]
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Column("password")]
        [Required(ErrorMessage = "Password may not be empty")]
        public string Password { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Column("salt")]
        public string Salt { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Column("email")]
        public string Email { get; set; }
        /// <summary>
        /// Rating
        /// </summary>
        [Column("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// Profile picture URL
        /// </summary>
        [Column("profilePictureUrl")]
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// Confirmed
        /// </summary>
        [Column("confirmed")]
        public bool Confirmed { get; set; }
    }
}
