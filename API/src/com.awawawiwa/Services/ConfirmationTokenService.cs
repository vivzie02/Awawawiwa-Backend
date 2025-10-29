using com.awawawiwa.Data.Context;
using com.awawawiwa.Data.Entities;
using System;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// ConfirmationTokenService
    /// </summary>
    public class ConfirmationTokenService : IConfirmationTokenService
    {
        private readonly ConfirmationTokenContext _confirmationTokenContext;

        /// <summary>
        /// ConfirmationTokenService
        /// </summary>
        /// <param name="confirmationTokenContext"></param>
        public ConfirmationTokenService(ConfirmationTokenContext confirmationTokenContext)
        {
            _confirmationTokenContext = confirmationTokenContext;
        }

        /// <summary>
        /// GenerateTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GenerateTokenAsync(Guid userId)
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
