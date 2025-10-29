using com.awawawiwa.Common.Constants;
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
            var category = QuestionCategory.IsValid(questionInputDTO.Category) ? questionInputDTO.Category : null;

            return new QuestionEntity
            {
                QuestionId = Guid.NewGuid(),
                Question = questionInputDTO.Question,
                Answer = questionInputDTO.Answer,
                Category = category
            };
        }

        /// <summary>
        /// ToDTO
        /// </summary>
        /// <param name="questionEntity"></param>
        /// <returns></returns>
        public static QuestionOutputDTO ToDTO(QuestionEntity questionEntity)
        {
            return new QuestionOutputDTO
            {
                QuestionId = questionEntity.QuestionId,
                Question = questionEntity.Question,
                Answer = questionEntity.Answer,
                Category = questionEntity.Category,
                AuthorId = questionEntity.AuthorId
            };
        }
    }
}
