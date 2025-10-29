using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.awawawiwa.Data.Entities
{
    /// <summary>
    /// ConfirmationTokenEntity
    /// </summary>
    public class ConfirmationTokenEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        [Column("userId")]
        public Guid UserId { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        [Column("token")]
        public string Token { get; set; }
        /// <summary>
        /// Expiration
        /// </summary>
        [Column("expiration")]
        public DateTime Expiration { get; set; }
    }
}
