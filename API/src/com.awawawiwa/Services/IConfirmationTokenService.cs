using System;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    public interface IConfirmationTokenService
    {
        /// <summary>
        /// GenerateTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GenerateTokenAsync(Guid userId);
    }
}
