using com.awawawiwa.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace com.awawawiwa.Data.Context
{
    public class ConfirmationTokenContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationTokenContext"/> class using the specified options.
        /// </summary>
        /// <remarks>This constructor is typically used to configure the context with specific settings,
        /// such as connection strings or database providers.</remarks>
        /// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
        public ConfirmationTokenContext(DbContextOptions<ConfirmationTokenContext> options) : base(options)
        {
        }

        /// <summary>
        /// ConfirmationTokens
        /// </summary>
        public DbSet<ConfirmationTokenEntity> ConfirmationTokens { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfirmationTokenEntity>().ToTable("confirmation_tokens");
            base.OnModelCreating(modelBuilder);
        }
    }
}
