using com.awawawiwa.Constants;
using com.awawawiwa.Data.Context;
using com.awawawiwa.DTOs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace com.awawawiwa.Services.Tests
{
    public class QuestionServiceTests
    {
        [Theory]
        [InlineData("Was ist die Hauptstadt von Deutschland?", "Berlin", QuestionCategory.Geography, true)]
        [InlineData("Was ist die Hauptstadt von Deutschland?", "Berlin", "IncorrectCategory", false)]
        [InlineData("", "Berlin", QuestionCategory.Geography, false)]
        [InlineData("Test", "", QuestionCategory.Geography, false)]
        public async Task CreateQuestionAsyncTest(
            string question,
            string answer,
            string category,
            bool expectedSuccess
            )
        {
            // Arrange
            var options = new DbContextOptionsBuilder<QuestionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new QuestionContext(options);
            var service = new QuestionService(context);

            var questionInput = new QuestionInputDTO
            {
                Question = question,
                Answer = answer,
                Category = category
            };
            var userId = Guid.NewGuid().ToString();

            // Act
            var result = await service.CreateQuestionAsync(questionInput, userId);

            // Assert
            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData("Was ist die Hauptstadt von Deutschland?", "Berlin", QuestionCategory.Geography)]
        public async Task GetQuestionByIdAsyncTest(
            string question,
            string answer,
            string category)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<QuestionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new QuestionContext(options);
            var service = new QuestionService(context);

            var questionInput = new QuestionInputDTO
            {
                Question = question,
                Answer = answer,
                Category = category
            };
            var userId = Guid.NewGuid().ToString();

            // Act
            await service.CreateQuestionAsync(questionInput, userId);

            var result = await service.GetQuestionByIdAsync((await context.Questions.FirstAsync()).QuestionId);

            // Assert
            Assert.Equal(question, result.Question);
            Assert.Equal(answer, result.Answer);
            Assert.Equal(category, result.Category);
        }

        [Theory]
        [InlineData("Was ist die Hauptstadt von Deutschland?", "Berlin", QuestionCategory.Geography, "7a648e17-f424-4013-a88a-4a532afd9e19", "7a648e17-f424-4013-a88a-4a532afd9e19", true)]
        [InlineData("Question", "Berlin", QuestionCategory.Geography, "7a648e17-f424-4013-a88a-4a532afd9e19", "78dc008c-c760-4e17-b1f9-b09cff062286", false)]
        public async Task DeleteQuestionByIdAsyncTest(
            string question,
            string answer,
            string category,
            string authorId,
            string userId,
            bool expectedSuccess)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<QuestionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new QuestionContext(options);
            var service = new QuestionService(context);

            var questionInput = new QuestionInputDTO
            {
                Question = question,
                Answer = answer,
                Category = category
            };

            // Act
            await service.CreateQuestionAsync(questionInput, authorId);

            var questionObject = (await service.GetQuestionsByUserIdAsync(Guid.Parse(authorId)))[0];

            var result = await service.DeleteQuestionByIdAsync(questionObject.QuestionId, userId.ToString());

            // Assert
            Assert.Equal(expectedSuccess, result.Success);
        }

        [Theory]
        [InlineData("TestFrage1", "Antwort1", "Antwort2", QuestionCategory.Geography, true)]
        [InlineData("TestFrage1", "Antwort1", "", QuestionCategory.Geography, false)]
        public async Task UpdateQuestionsAsyncTest(
            string question,
            string answer,
            string newAnswer,
            string category,
            bool expectedSuccess)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<QuestionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new QuestionContext(options);
            var service = new QuestionService(context);

            var questionInput = new QuestionInputDTO
            {
                Question = question,
                Answer = answer,
                Category = category
            };

            var newQuestion = new QuestionInputDTO
            {
                Question = question,
                Answer = newAnswer,
                Category = category
            };

            var user = Guid.NewGuid();

            // Act
            await service.CreateQuestionAsync(questionInput, user.ToString());

            var result = await service.UpdateQuestionAsync((await context.Questions.FirstAsync()).QuestionId, user.ToString(), newQuestion);

            // Assert
            Assert.Equal(expectedSuccess, result.Success);
        }
    }
}