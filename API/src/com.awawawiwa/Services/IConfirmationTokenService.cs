using com.awawawiwa.DTOs;
using System;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    public interface IConfirmationTokenService
    {
        /// <summary>
        /// SendConfirmationMail
        /// </summary>
        /// <param name="userInputDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SendConfirmationMail(CreateUserInputDTO userInputDto, Guid userId);

        /// <summary>
        /// ConfirmToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmToken(string token);
    }
}
