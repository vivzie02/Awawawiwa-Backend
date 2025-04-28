namespace com.awawawiwa.Models
{
    /// <summary>
    /// QuestionOperationResult
    /// </summary>
    public class QuestionOperationResult
    {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// ErrorCode
        /// </summary>
        public string ErrorCode { get; set; } // like "QuestionNotFound", "InvalidQuestion"
        /// <summary>
        /// ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
