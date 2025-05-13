using com.awawawiwa.DTOs;
using com.awawawiwa.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    public interface IQuestionService
    {
        /// <summary>
        /// create a new question
        /// </summary>
        /// <param name="questionInputDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<QuestionOperationResult> CreateQuestionAsync(QuestionInputDTO questionInputDTO, string userId);

        /// <summary>
        /// GetQuestionByIdAsync
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Task<QuestionOutputDTO> GetQuestionByIdAsync(Guid questionId);

        /// <summary>
        /// DeleteQuestionByIdAsync
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public Task<QuestionOperationResult> DeleteQuestionByIdAsync(Guid questionId, string loggedInUser);

        /// <summary>
        /// GetRandomQuestion
        /// </summary>
        /// <returns></returns>
        public Task<QuestionOutputDTO> GetRandomQuestionAsync();

        /// <summary>
        /// GetRandomQuestion by category
        /// </summary>
        /// <returns></returns>
        public Task<QuestionOutputDTO> GetRandomQuestionByCategoryAsync(string category);

        /// <summary>
        /// Update question
        /// </summary>
        /// <returns></returns>
        public Task<QuestionOperationResult> UpdateQuestionAsync(Guid questionId, string loggedInUser, QuestionInputDTO questionInputDTO);

        /// <summary>
        /// Get questions by user id
        /// </summary>
        /// <returns></returns>
        public Task<List<QuestionOutputDTO>> GetQuestionsByUserIdAsync(Guid userId);
    }
}
