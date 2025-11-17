using com.awawawiwa.Data.Context;
using com.awawawiwa.Data.Entities;
using com.awawawiwa.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace com.awawawiwa.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// confirm user email for the given user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserOperationResult> ConfirmEmailAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            user.Confirmed = true;

            await _context.SaveChangesAsync();

            return new UserOperationResult
            {
                Success = true
            };
        }

        public async Task SaveAsync(UserEntity user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
