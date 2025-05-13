using System.ComponentModel.DataAnnotations;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// LoginUserInputDTO
    /// </summary>
    public class LoginUserInputDTO
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
    }
}
