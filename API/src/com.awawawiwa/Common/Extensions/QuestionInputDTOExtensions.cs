using com.awawawiwa.Common.Constants;
using com.awawawiwa.DTOs;
using static com.awawawiwa.Common.Constants.Constants;

namespace com.awawawiwa.Common.Extensions
{
    /// <summary>
    /// Extensions for the QuestionInputDTO
    /// </summary>
    public static class QuestionInputDTOExtensions
    {

        /// <summary>
        /// Check if the QuestionInputDTO is valid
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static bool IsValid(this QuestionInputDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Question) ||
                string.IsNullOrWhiteSpace(dto.Answer) ||
                string.IsNullOrWhiteSpace(dto.Category) ||
                dto.Question.Length > MAX_QUESTION_LENGTH ||
                dto.Answer.Length > MAX_QUESTION_LENGTH ||
                !QuestionCategory.IsValid(dto.Category))
            {
                return false;
            }
            return true;
        }
    }
}
