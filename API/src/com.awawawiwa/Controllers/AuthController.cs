using com.awawawiwa.DTOs;
using com.awawawiwa.Services;
using IO.Swagger.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace com.awawawiwa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfirmationTokenService _confirmationTokenService;

        /// <summary>
        /// AuthController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="confirmationTokenService"></param>
        public AuthController(ILogger<AuthController> logger, IConfirmationTokenService confirmationTokenService)
        {
            _logger = logger;
            _confirmationTokenService = confirmationTokenService;
        }

        /// <summary>
        /// SendConfirmationMail
        /// </summary>
        /// <param name="userInputDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("sendConfirmationMail")]
        [ValidateModelState]
        [SwaggerOperation("SendConfirmationMail")]
        [SwaggerResponse(200, "Login successful")]
        [SwaggerResponse(401, "Invalid username or password")]
        public IActionResult SendConfirmationMail([FromBody] CreateUserInputDTO userInputDto, [FromQuery] Guid userId)
        {
            _logger.LogInformation(">>> Call SendConfirmationMail");
            _confirmationTokenService.SendConfirmationMail(userInputDto, userId);
            return Ok();
        }

        /// <summary>
        /// ConfirmToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("confirm")]
        [ValidateModelState]
        [SwaggerOperation("ConfirmToken")]
        [SwaggerResponse(200, "confirmation successful")]
        [SwaggerResponse(401, "Invalid token")]
        public async Task<IActionResult> ConfirmToken([FromQuery] string token)
        {
            _logger.LogInformation(">>> Call ConfirmToken");
            await _confirmationTokenService.ConfirmToken(token);
            return Ok();
        }
    }
}
