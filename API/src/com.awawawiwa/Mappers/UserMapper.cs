using com.awawawiwa.Data.Entities;
using com.awawawiwa.DTOs;
using com.awawawiwa.Security;
using static com.awawawiwa.Constants.Constants;

namespace com.awawawiwa.Mappers
{
    /// <summary>
    /// UserMapper
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// ToEntity
        /// </summary>
        /// <param name="userInputDTO"></param>
        /// <returns></returns>
        public static UserEntity ToEntity(CreateUserInputDTO userInputDTO)
        {
            var salt = PasswordHasherService.GenerateSalt();
            var hashedPassword = PasswordHasherService.ComputeHash(userInputDTO.Password, salt, HASHING_ITERATIONS);

            return new UserEntity
            {
                Username = userInputDTO.Username,
                Salt = salt,
                Password = hashedPassword,
                Email = userInputDTO.Email,
            };
        }
    }
}
