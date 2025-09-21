using com.awawawiwa.Constants;
using com.awawawiwa.Data.Entities;
using com.awawawiwa.DTOs;

namespace com.awawawiwa.Extensions
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
                !QuestionCategory.IsValid(dto.Category))
            {
                return false;
            }
            return true;
        }
    }
}
