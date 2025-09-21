using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using com.awawawiwa.Models;
using com.awawawiwa.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
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
            var validationResult = await IsUserInputValid(userInput);

            if(!validationResult.Success)
            {
                return validationResult;
            }

            await SaveUser(userInput);
            return validationResult;
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

            if (userEntity is null)
            {
                return new UserDataOutputDTO
                {
                    Id = Guid.Empty,
                    Username = null,
                    Email = null
                };
            }

            var userDataOutputDTO = new UserDataOutputDTO
            {
                Id = userEntity.UserId,
                Username = userEntity.Username,
                Email = userEntity.Email,
                Rating = userEntity.Rating,
                ProfilePicture = userEntity.ProfilePictureUrl
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

            var isLoggedIn = !string.IsNullOrEmpty(jti) && !_revokedTokensService.IsTokenRevoked(jti);
            return new UserLoggedInStatusOutputDTO
            {
                IsLoggedIn = isLoggedIn
            };
        }

        /// <summary>
        /// UploadProfilePictureAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="profilePicture"></param>
        /// <returns></returns>
        public async Task<UserOperationResult> UploadProfilePictureAsync(Guid userId, IFormFile profilePicture)
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

            if (profilePicture == null || profilePicture.Length == 0)
            {
                return new UserOperationResult
                {
                    ErrorCode = "InvalidProfilePicture",
                    ErrorMessage = "No profile picture provided",
                    Success = false
                };
            }

            var extension = Path.GetExtension(profilePicture.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedTypes.Contains(extension))
            {
                return new UserOperationResult
                {
                    Success = false,
                    ErrorCode = "InvalidFileType",
                    ErrorMessage = "Only image files are allowed"
                };
            }

            var uploadDir = Path.Combine("wwwroot", "uploads", "profilePictures");
            Directory.CreateDirectory(uploadDir);

            var path = Path.Combine("wwwroot/uploads/profilePictures", fileName);

            //remove old picture
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                var oldPath = Path.Combine("wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            // Save file path or URL to database for the user
            var fileUrl = $"/uploads/profilePictures/{fileName}";
            await SaveProfilePictureUrlAsync(userId, fileUrl);

            return new UserOperationResult
            {
                Success = true,
                UserData = new UserDataOutputDTO
                {
                    ProfilePicture = fileUrl
                }
            };
        }

        private async Task SaveProfilePictureUrlAsync(Guid userId, string filePath)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.ProfilePictureUrl = filePath;
            await _context.SaveChangesAsync();
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

        private async Task<UserOperationResult> IsUserInputValid(CreateUserInputDTO userInput)
        {
            if(string.IsNullOrEmpty(userInput.Username) ||
               string.IsNullOrEmpty(userInput.Password) ||
               string.IsNullOrEmpty(userInput.Email))
            {
                return new UserOperationResult
                {
                    ErrorCode = "MissingFields",
                    ErrorMessage = "Username, Password and Email are required",
                    Success = false
                };
            }

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
            else
            {
                return new UserOperationResult
                {
                    Success = true
                };
            }
        }
    }
}
