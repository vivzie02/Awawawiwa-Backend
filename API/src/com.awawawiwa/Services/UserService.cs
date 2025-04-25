using com.awawawiwa.Data.Context;
using com.awawawiwa.Data.Entities;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using com.awawawiwa.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.SqlClient;
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

        /// <summary>
        /// UserService
        /// </summary>
        public UserService(UserContext userContext, IJwtService jwtService)
        {
            _context = userContext;
            _jwtService = jwtService;
        }

        /// <summary>
        /// create a new user
        /// </summary>
        /// <param name="userInput"></param>
        public async Task CreateUserAsync(UserInputDTO userInput)
        {
            //Check if user exists
            if(UsernameExists(userInput.Username))
            {
                throw new ArgumentException("User already exists");
            }
            //Check if email exists
            if (EmailExists(userInput.Email))
            {
                throw new ArgumentException("Email already exists");
            }
            if (!IsValidEmail(userInput.Email))
            {
                throw new ArgumentException("Invalid email format");
            }

            await SaveUser(userInput);
        }

        /// <summary>
        /// delete user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user == null)
            {
                throw new ObjectNotFoundException("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// loign user
        /// </summary>
        /// <param name="userInputDTO"></param>
        /// <returns></returns>
        /// <exception cref="ObjectNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<string> LoginUserAsync(UserInputDTO userInputDTO)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == userInputDTO.Username);

            if (userEntity == null)
            {
                throw new ObjectNotFoundException("Incorrect Username or Password");
            }

            if(!PasswordHasherService.VerifyPassword(userInputDTO.Password, userEntity.Salt, userEntity.Password))
            {
                throw new UnauthorizedAccessException("Incorrect Username or Password");
            }

            var token = _jwtService.GenerateToken(userEntity.UserId);
            return token;

        }

        private async Task SaveUser(UserInputDTO userInput)
        {
            var userEntity = UserMapper.ToEntity(userInput);
            userEntity.UserId = Guid.NewGuid(); // Generate a new GUID for the user ID

            _context.Add(userEntity);
            await _context.SaveChangesAsync();
        }
        
        private bool UsernameExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }

        private bool EmailExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, EMAIL_REGEX);
        }
    }
}
