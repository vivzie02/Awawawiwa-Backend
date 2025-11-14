using com.awawawiwa.Data.Context;
using com.awawawiwa.Data.Entities;
using com.awawawiwa.Data.Repositories;
using com.awawawiwa.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// ConfirmationTokenService
    /// </summary>
    public class ConfirmationTokenService : IConfirmationTokenService
    {
        private readonly ConfirmationTokenContext _confirmationTokenContext;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ConfirmationTokenService> _logger;

        /// <summary>
        /// ConfirmationTokenService
        /// </summary>
        /// <param name="confirmationTokenContext"></param>
        /// <param name="emailService"></param>
        /// <param name="userRepository"></param>
        public ConfirmationTokenService(ConfirmationTokenContext confirmationTokenContext, IEmailService emailService, IUserRepository userRepository, ILogger<ConfirmationTokenService> logger)
        {
            _confirmationTokenContext = confirmationTokenContext;
            _emailService = emailService;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// SendConfirmationMail
        /// </summary>
        /// <param name="userInputDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SendConfirmationMail(CreateUserInputDTO userInputDto, Guid userId)
        {
            //create user confirmation token
            var confirmationToken = await GenerateTokenAsync(userId);

            var mailInputDto = new SendConfirmationEmailInputDto
            {
                Recipient = userInputDto.Email,
                Token = confirmationToken,
            };

            await _emailService.SendConfirmationEmailAsync(mailInputDto);
        }

        /// <summary>
        /// ValidateTokenAsync
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmToken(string token)
        {
            try
            {
                var existingToken = await _confirmationTokenContext.ConfirmationTokens.FirstOrDefaultAsync(t => string.Equals(t.Token, token));
                if (existingToken == null || existingToken.Expiration < DateTime.UtcNow)
                {
                    return false;
                }
                // Optionally, you can remove the token after validation
                _confirmationTokenContext.Remove(existingToken);
                await _confirmationTokenContext.SaveChangesAsync();

                //confirm user
                await _userRepository.ConfirmEmailAsync(existingToken.UserId);

                return true;
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error confirming token");
                return false;
            }
            
        }

        /// <summary>
        /// GenerateTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<string> GenerateTokenAsync(Guid userId)
        {
            var token = new ConfirmationTokenEntity
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                Expiration = DateTime.UtcNow.AddHours(24)
            };

            _confirmationTokenContext.Add(token);
            await _confirmationTokenContext.SaveChangesAsync();

            return token.Token;
        }
    }
}
