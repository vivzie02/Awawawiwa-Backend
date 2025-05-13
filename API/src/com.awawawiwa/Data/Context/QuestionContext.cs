using com.awawawiwa.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace com.awawawiwa.Data.Context
{
    /// <summary>
    /// QuestionContext
    /// </summary>
    public class QuestionContext : DbContext
    {
        /// <summary>
        /// QuestionContext
        /// </summary>
        /// <param name="options"></param>
        public QuestionContext(DbContextOptions<QuestionContext> options) : base(options)
        {
        }

        /// <summary>
        /// Questions
        /// </summary>
        public DbSet<QuestionEntity> Questions { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionEntity>().ToTable("questions");
            base.OnModelCreating(modelBuilder);
        }
    }
}
