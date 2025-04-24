using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// UserService
        /// </summary>
        public UserService(UserContext userContext)
        {
            _context = userContext;
        }

        /// <summary>
        /// create a new user
        /// </summary>
        /// <param name="userInput"></param>
        public async Task CreateUserAsync(UserInputDTO userInput)
        {
            var userEntity = UserMapper.ToEntity(userInput);
            userEntity.UserId = Guid.NewGuid(); // Generate a new GUID for the user ID
            try
            {
                _context.Add(userEntity);
                await _context.SaveChangesAsync();

                return;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw;
            }
        }
    }
}
