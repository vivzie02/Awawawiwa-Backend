using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using com.awawawiwa.Mappers;
using Microsoft.AspNetCore.Http;
using System;
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
        /// <returns></returns>
        public async Task CreateQuestionAsync(QuestionInputDTO questionInputDTO, string userId)
        {
            var questionEntity = QuestionMapper.ToEntity(questionInputDTO);

            // store logged in user as author
            questionEntity.AuthorId = Guid.Parse(userId);

            if (_context.Questions.Any(question => string.Equals(question.Question, questionInputDTO.Question)))
            {
                throw new ArgumentException("Question already exists");
            }

            _context.Questions.Add(questionEntity);
            await _context.SaveChangesAsync();
        }
    }
}
