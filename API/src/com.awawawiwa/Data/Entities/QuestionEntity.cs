using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.awawawiwa.Data.Entities
{
    /// <summary>
    /// QuestionEntity
    /// </summary>
    [Table("questions")]
    public class QuestionEntity
    {
        /// <summary>
        /// QuestionId
        /// </summary>
        [Key]
        [Column("questionId")]
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Question
        /// </summary>
        [Column("question")]
        [Required(ErrorMessage = "Question may not be empty")]
        public string Question { get; set; }

        /// <summary>
        /// Answer
        /// </summary>
        [Column("answer")]
        [Required(ErrorMessage = "Answer may not be empty")]
        public string Answer { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [Column("category")]
        [Required(ErrorMessage = "Category may not be empty")]
        public string Category { get; set; }

        /// <summary>
        /// AuthorId
        /// </summary>
        [ForeignKey("UserEntity")]
        [Column("authorId")]
        public Guid AuthorId { get; set; }
    }
}
