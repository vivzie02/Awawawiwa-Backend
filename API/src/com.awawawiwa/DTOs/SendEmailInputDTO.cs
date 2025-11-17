namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// SendEmailInputDTO
    /// </summary>
    public class SendEmailInputDTO
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// Recipient
        /// </summary>
        public string Recipient { get; set; } = string.Empty;
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;
        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}
