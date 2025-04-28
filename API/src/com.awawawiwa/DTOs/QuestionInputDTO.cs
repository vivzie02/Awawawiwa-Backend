using System.ComponentModel.DataAnnotations;

namespace com.awawawiwa.DTOs
{
    /// <summary>
    /// QuestionInputDTO
    /// </summary>
    public class QuestionInputDTO
    {
        /// <summary>
        /// Gets or Sets Question
        /// </summary>
        [Required(ErrorMessage = "Question is required")]
        public string Question { get; set; }

        /// <summary>
        /// Gets or Sets Answer
        /// </summary>
        [Required(ErrorMessage = "Answer is required")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
    }
}
