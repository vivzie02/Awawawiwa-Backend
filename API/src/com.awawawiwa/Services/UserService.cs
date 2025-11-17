using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using com.awawawiwa.Models;
using com.awawawiwa.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static com.awawawiwa.Common.Constants.Constants;

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
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;
        private readonly IConfirmationTokenService _confirmationTokenService;

        /// <summary>
        /// UserService
        /// </summary>
        public UserService(UserContext userContext, IJwtService jwtService, IRevokedTokensService revokedTokensService, ILogger<UserService> logger, IEmailService emailService, IConfirmationTokenService confirmationTokenService)
        {
            _context = userContext;
            _jwtService = jwtService;
            _revokedTokensService = revokedTokensService;
            _logger = logger;
            _emailService = emailService;
            _confirmationTokenService = confirmationTokenService;
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
                _logger.LogWarning("User creation failed");
                return validationResult;
            }

            var userId = Guid.NewGuid();

            try
            {
                _logger.LogInformation("Send confirmation email to new user");
                //send account confirmation email
                await _confirmationTokenService.SendConfirmationMail(userInput, userId);
            }catch(Exception ex)
            {
                _logger.LogError($"Failed to send confirmation email: {ex.Message}");
                return new UserOperationResult
                {
                    ErrorCode = "EmailSendFailed",
                    ErrorMessage = "Failed to send confirmation email",
                    Success = false
                };
            }

            await SaveUser(userInput, userId);

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
                _logger.LogWarning("User deletion failed: User not found");

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
                _logger.LogInformation("Login failed: User not found");
                return null;
            }

            if (!PasswordHasherService.VerifyPassword(userInputDTO.Password, userEntity.Salt, userEntity.Password))
            {
                _logger.LogInformation("Login failed: Incorrect password");
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
                _logger.LogInformation("GetUserDataAsync: User with not found");

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
                _logger.LogWarning("UploadProfilePictureAsync failed: User not found");

                return new UserOperationResult
                {
                    ErrorCode = "UserNotFound",
                    ErrorMessage = "User not found",
                    Success = false
                };
            }

            if (profilePicture == null || profilePicture.Length == 0)
            {
                _logger.LogWarning("UploadProfilePictureAsync failed: No profile picture provided");

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
                _logger.LogWarning("UploadProfilePictureAsync failed: Invalid file type for");

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
                _logger.LogInformation("Removing old profile picture");

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

        /// <summary>
        /// sets the user email as confirmed for the given user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserOperationResult> ConfirmUserEmailAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            user.Confirmed = true;

            await _context.SaveChangesAsync();

            return new UserOperationResult
            {
                Success = true
            };
        }

        private async Task SaveProfilePictureUrlAsync(Guid userId, string filePath)
        {
            _logger.LogInformation("Saving profile picture URL");

            var user = await _context.Users.FindAsync(userId);

            user.ProfilePictureUrl = filePath;
            await _context.SaveChangesAsync();
        }

        private async Task SaveUser(CreateUserInputDTO userInput, Guid userId)
        {
            _logger.LogInformation("Saving new user to database");

            var userEntity = UserMapper.ToEntity(userInput);
            userEntity.UserId = userId;

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
                _logger.LogWarning("User input validation failed: Missing required fields");

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
                _logger.LogWarning("User input validation failed: Username already exists");

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
                _logger.LogWarning("User input validation failed: Email already in use");

                return new UserOperationResult
                {
                    ErrorCode = "EmailTaken",
                    ErrorMessage = "Email already in use",
                    Success = false
                };
            }
            if (!IsValidEmail(userInput.Email))
            {
                _logger.LogWarning("User input validation failed: Invalid email format");

                return new UserOperationResult
                {
                    ErrorCode = "InvalidEmail",
                    ErrorMessage = "Email is not valid",
                    Success = false
                };
            }
            if (userInput.Password.Length < MIN_PASSWORD_LENGTH)
            {
                _logger.LogWarning("User input validation failed: Password too short");

                return new UserOperationResult
                {
                    ErrorCode = "PasswordTooShort",
                    ErrorMessage = $"Password must be at least {MIN_PASSWORD_LENGTH} characters long",
                    Success = false
                };
            }

            return new UserOperationResult
            {
                Success = true
            };
        }
    }
}
