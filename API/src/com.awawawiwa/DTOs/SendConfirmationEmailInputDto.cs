namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// SendConfirmationEmailInputDto
    /// </summary>
    public class SendConfirmationEmailInputDto : SendEmailInputDTO
    {
        /// <summary>
        /// ConfirmationLink
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
