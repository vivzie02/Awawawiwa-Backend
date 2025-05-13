using com.awawawiwa.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace com.awawawiwa.Data.Context
{
    /// <summary>
    /// UserContext
    /// </summary>
    public class UserContext : DbContext
    {
        /// <summary>
        /// UserContext
        /// </summary>
        /// <param name="options"></param>
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().ToTable("users");
            base.OnModelCreating(modelBuilder);
        }
    }
}
