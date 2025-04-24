using com.awawawiwa.DTOs;
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
        Task CreateUserAsync(UserInputDTO userInput);
    }
}
