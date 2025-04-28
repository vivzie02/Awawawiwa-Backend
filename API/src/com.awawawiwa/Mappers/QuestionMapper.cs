using com.awawawiwa.Constants;
using com.awawawiwa.Data.Entities;
using com.awawawiwa.DTOs;
using com.awawawiwa.Security;
using System;

namespace com.awawawiwa.Mappers
{
    /// <summary>
    /// QuestionMapper
    /// </summary>
    public static class QuestionMapper
    {
        /// <summary>
        /// ToEntity
        /// </summary>
        /// <param name="questionInputDTO"></param>
        /// <returns></returns>
        public static QuestionEntity ToEntity(QuestionInputDTO questionInputDTO)
        {

            var category = QuestionCategory.IsValid(questionInputDTO.Category) ? questionInputDTO.Category : throw new ArgumentException("Invalid category");

            return new QuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Question = questionInputDTO.Question,
                Answer = questionInputDTO.Answer,
                Category = category
            };
        }
    }
}
