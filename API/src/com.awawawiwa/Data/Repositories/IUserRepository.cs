using com.awawawiwa.Data.Entities;
using com.awawawiwa.Models;
using System;
using System.Threading.Tasks;

namespace com.awawawiwa.Data.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(Guid userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<UserOperationResult> ConfirmEmailAsync(Guid userId);
        Task SaveAsync(UserEntity user);
    }
}
