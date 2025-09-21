using com.awawawiwa.DTOs;

namespace com.awawawiwa.Models
{
    /// <summary>
    /// UserOperationResult
    /// </summary>
    public class UserOperationResult
    {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// ErrorCode
        /// </summary>
        public string ErrorCode { get; set; } // like "UsernameTaken", "EmailTaken", "InvalidEmail"
        /// <summary>
        /// ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// UserData
        /// </summary>
        public UserDataOutputDTO UserData { get; set; }
    }
}
