using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using com.awawawiwa.Models;
using com.awawawiwa.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static com.awawawiwa.Constants.Constants;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// UserService
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Context
        /// </summary>
        private readonly UserContext _context;
        private readonly IJwtService _jwtService;
        private readonly IRevokedTokensService _revokedTokensService;

        /// <summary>
        /// UserService
        /// </summary>
        public UserService(UserContext userContext, IJwtService jwtService, IRevokedTokensService revokedTokensService)
        {
            _context = userContext;
            _jwtService = jwtService;
            _revokedTokensService = revokedTokensService;
        }

        /// <summary>
        /// create a new user
        /// </summary>
        /// <param name="userInput"></param>
        public async Task<UserOperationResult> CreateUserAsync(CreateUserInputDTO userInput)
        {
            //Check if user exists
            if (await UsernameExists(userInput.Username))
            {
                return new UserOperationResult
                {
                    ErrorCode = "UsernameTaken",
                    ErrorMessage = "Username already exists",
                    Success = false
                };
            }
            //Check if email exists
            if (await EmailExists(userInput.Email))
            {
                return new UserOperationResult
                {
                    ErrorCode = "EmailTaken",
                    ErrorMessage = "Email already in use",
                    Success = false
                };
            }
            if (!IsValidEmail(userInput.Email))
            {
                return new UserOperationResult
                {
                    ErrorCode = "InvalidEmail",
                    ErrorMessage = "Email is not valid",
                    Success = false
                };
            }

            await SaveUser(userInput);
            return new UserOperationResult
            {
                Success = true
            };
        }

        /// <summary>
        /// delete user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserOperationResult> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new UserOperationResult
                {
                    ErrorCode = "UserNotFound",
                    ErrorMessage = "User not found",
                    Success = false
                };
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new UserOperationResult
            {
                Success = true
            };
        }

        /// <summary>
        /// loign user
        /// </summary>
        /// <param name="userInputDTO"></param>
        /// <returns></returns>
        public async Task<LoginUserOutputDTO> LoginUserAsync(LoginUserInputDTO userInputDTO)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == userInputDTO.Username);

            if (userEntity == null)
            {
                return null;
            }

            if (!PasswordHasherService.VerifyPassword(userInputDTO.Password, userEntity.Salt, userEntity.Password))
            {
                return null;
            }

            var token = _jwtService.GenerateToken(userEntity.UserId);

            return new LoginUserOutputDTO
            {
                UserId = userEntity.UserId,
                Token = token
            };
        }

        /// <summary>
        /// logout user by id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void LogoutUser(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var jti = jwtToken?.Id; // Unique token ID

            if (string.IsNullOrEmpty(jti))
            {
                throw new ArgumentException("Invalid token");
            }

            _revokedTokensService.RevokeToken(jti);
        }

        /// <summary>
        /// GetUserDataAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDataOutputDTO> GetUserDataAsync(Guid userId)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            var userDataOutputDTO = new UserDataOutputDTO
            {
                Id = userEntity.UserId,
                Username = userEntity.Username,
                Email = userEntity.Email,
                Rating = userEntity.Rating
            };

            return userDataOutputDTO;
        }

        /// <summary>
        /// IsUserLoggedIn
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserLoggedInStatusOutputDTO IsUserLoggedIn(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var jti = jwtToken?.Id; // Unique token ID

            return new UserLoggedInStatusOutputDTO
            {
                IsLoggedIn = !string.IsNullOrEmpty(jti) && !_revokedTokensService.IsTokenRevoked(jti)
            };
        }

        private async Task SaveUser(CreateUserInputDTO userInput)
        {
            var userEntity = UserMapper.ToEntity(userInput);
            userEntity.UserId = Guid.NewGuid(); // Generate a new GUID for the user ID

            _context.Add(userEntity);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(e => e.Username == username);
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(e => e.Email == email);
        }

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, EMAIL_REGEX);
        }
    }
}
