using com.awawawiwa.DTOs;
using System.Threading.Tasks;

namespace com.awawawiwa.Services
{
    public interface IQuestionService
    {
        /// <summary>
        /// create a new question
        /// </summary>
        /// <param name="questionInputDTO"></param>
        /// <returns></returns>
        Task CreateQuestionAsync(QuestionInputDTO questionInputDTO, string userId);
    }
}
