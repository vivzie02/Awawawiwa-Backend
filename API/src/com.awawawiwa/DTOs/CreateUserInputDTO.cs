using System.ComponentModel.DataAnnotations;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// CreateUserInputDTO
    /// </summary>
    public class CreateUserInputDTO
    {
        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or Sets Password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
