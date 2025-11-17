namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// SendEmailOutputDTO
    /// </summary>
    public class SendEmailOutputDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether the email was sent successfully.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Gets or sets the message associated with the email sending operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
