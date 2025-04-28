using System;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// QuestionOutputDTO
    /// </summary>
    public class QuestionOutputDTO
    {
        /// <summary>
        /// QuestionId
        /// </summary>
        public Guid QuestionId { get; set; }
        /// <summary>
        /// Question
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// Answer
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// AuthorId
        /// </summary>
        public Guid AuthorId { get; set; }
    }
}
