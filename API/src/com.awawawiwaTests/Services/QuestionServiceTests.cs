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
    }
}