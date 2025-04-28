using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using com.awawawiwa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    /// <summary>
    /// QuestionService
    /// </summary>
    public class QuestionService : IQuestionService
    {
        private readonly QuestionContext _context;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="questionContext"></param>
        public QuestionService(QuestionContext questionContext)
        {
            _context = questionContext;
        }

        /// <summary>
        /// create a new question
        /// </summary>
        /// <param name="questionInputDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<QuestionOperationResult> CreateQuestionAsync(QuestionInputDTO questionInputDTO, string userId)
        {
            var questionEntity = QuestionMapper.ToEntity(questionInputDTO);

            // store logged in user as author
            questionEntity.AuthorId = Guid.Parse(userId);

            var questionExists = await _context.Questions.AnyAsync(question => question.Question == questionInputDTO.Question);
            if (questionExists)
            {
                return new QuestionOperationResult
                {
                    ErrorCode = "QuestionExists",
                    ErrorMessage = "Question already exists",
                    Success = false
                };
            }

            _context.Questions.Add(questionEntity);
            await _context.SaveChangesAsync();

            return new QuestionOperationResult
            {
                Success = true
            };
        }

        /// <summary>
        /// GetQuestionByIdAsync
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public async Task<QuestionOutputDTO> GetQuestionByIdAsync(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                return null;
            }

            return QuestionMapper.ToDTO(question);
        }

        /// <summary>
        /// DeleteQuestionByIdAsync
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public async Task<QuestionOperationResult> DeleteQuestionByIdAsync(Guid questionId, string loggedInUser)
        {
            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                return new QuestionOperationResult
                {
                    ErrorCode = "QuestionNotFound",
                    ErrorMessage = "Question not found",
                    Success = false
                };
            }

            //only delete own questions 
            if (Guid.Parse(loggedInUser) != question.AuthorId)
            {
                return new QuestionOperationResult
                {
                    ErrorCode = "NotAuthorized",
                    ErrorMessage = "You are not authorized to delete this question",
                    Success = false
                };
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return new QuestionOperationResult
            {
                Success = true
            };
        }

        /// <summary>
        /// GetRandomQuestion
        /// </summary>
        /// <returns></returns>
        public async Task<QuestionOutputDTO> GetRandomQuestionAsync()
        {
            var randomQuestion = await _context.Questions
                .OrderBy(q => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (randomQuestion == null)
            {
                return null;
            }

            return QuestionMapper.ToDTO(randomQuestion);
        }

        /// <summary>
        /// GetRandomQuestionByCategoryAsync
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<QuestionOutputDTO> GetRandomQuestionByCategoryAsync(string category)
        {
            var randomQuestion = await _context.Questions
                .Where(q => q.Category == category)
                .OrderBy(q => Guid.NewGuid())
                .FirstOrDefaultAsync();

            if (randomQuestion == null)
            {
                return null;
            }

            return QuestionMapper.ToDTO(randomQuestion);
        }

        /// <summary>
        /// UpdateQuesetionAsync
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="loggedInUser"></param>
        /// <param name="questionInputDTO"></param>
        /// <returns></returns>
        public async Task<QuestionOperationResult> UpdateQuestionAsync(Guid questionId, string loggedInUser, QuestionInputDTO questionInputDTO)
        {
            var question = await _context.Questions.FindAsync(questionId);

            if (question == null)
            {
                return new QuestionOperationResult
                {
                    ErrorCode = "QuestionNotFound",
                    ErrorMessage = "Question not found",
                    Success = false
                };
            }

            if (question.AuthorId != Guid.Parse(loggedInUser))
            {
                return new QuestionOperationResult
                {
                    ErrorCode = "NotAuthorized",
                    ErrorMessage = "You are not authorized to update this question",
                    Success = false
                };
            }

            //update the question fields
            question.Question = questionInputDTO.Question;
            question.Answer = questionInputDTO.Answer;
            question.Category = questionInputDTO.Category;

            await _context.SaveChangesAsync();

            return new QuestionOperationResult
            {
                Success = true
            };
        }

        /// <summary>
        /// GetQuestionsByUserIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<QuestionOutputDTO>> GetQuestionsByUserIdAsync(Guid userId)
        {
            var questionEntities = await _context.Questions.Where(question => question.AuthorId == userId).ToListAsync();

            if(questionEntities == null)
            {
                return null;
            }

            var questionOutputDtos = questionEntities.Select(QuestionMapper.ToDTO).ToList();
            return questionOutputDtos;
        }
    }
}
