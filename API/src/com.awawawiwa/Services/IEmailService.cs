using com.awawawiwa.DTOs;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// IEmailService
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// SendEmailAsync
        /// </summary>
        /// <returns></returns>
        Task<SendEmailOutputDTO> SendConfirmationEmailAsync(SendConfirmationEmailInputDto input);
    }
}
