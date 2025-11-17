using com.awawawiwa.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static com.awawawiwa.Common.Constants.UrlConstants;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// EmailService
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// HttpClient
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// EmailService
        /// </summary>
        /// <param name="httpClient"></param>
        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// SendEmailAsync
        /// </summary>
        /// <returns></returns>
        public async Task<SendEmailOutputDTO> SendConfirmationEmailAsync(SendConfirmationEmailInputDto input)
        {
            // Send the POST request as JSON
            var response = await _httpClient.PostAsJsonAsync($"{EMAIL_SERVICE_API}/api/email/sendConfirmationMail", input);

            // Throw an exception if the response was not successful
            response.EnsureSuccessStatusCode();

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new SendEmailOutputDTO
                {
                    IsSuccess = false,
                    Message = $"Email service returned status code {response.StatusCode}"
                };
            }

            return new SendEmailOutputDTO
            {
                IsSuccess = true,
                Message = "Email sent successfully"
            };
        }
    }
}
