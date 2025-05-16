using com.awawawiwa.DTOs;
using com.awawawiwa.Models;
using System;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// create a new user
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        Task<UserOperationResult> CreateUserAsync(CreateUserInputDTO userInput);

        /// <summary>
        /// create a new user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserOperationResult> DeleteUserAsync(Guid userId);

        /// <summary>
        /// login user
        /// </summary>
        /// <param name="userInputDTO"></param>
        /// <returns></returns>
        Task<LoginUserOutputDTO> LoginUserAsync(LoginUserInputDTO userInputDTO);

        /// <summary>
        /// logout user
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        void LogoutUser(string token);

        /// <summary>
        /// GetUserDataAsync
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserDataOutputDTO> GetUserDataAsync(Guid userId);
    }
}
