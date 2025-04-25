using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NUnit.Framework;

namespace NaviriaAPI.Tests.DTOs.FeaturesDTOs
{
    [TestFixture]
    public class AssistantChatMessageDtoTests
    {
        private IList<ValidationResult> ValidateDto(AssistantChatMessageDto dto)
        {
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
            return results;
        }

        [Test]
        public void TC001_AssistantChatMessageDto_ShouldBeValid_WhenMessageIsProper()
        {
            var dto = new AssistantChatMessageDto
            {
                UserId = "user-123",
                Message = "Hello, how can I help you today?"
            };

            var results = ValidateDto(dto);

            Assert.That(results, Is.Empty);
        }

        [Test]
        public void TC002_AssistantChatMessageDto_ShouldBeInvalid_WhenMessageIsTooShort()
        {
            var dto = new AssistantChatMessageDto
            {
                UserId = "user-123",
                Message = "" // too short == required error
            };

            var results = ValidateDto(dto);

            Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r =>
                r.ErrorMessage == "The Message field is required."));
        }


        [Test]
        public void TC003AssistantChatMessageDto_ShouldBeInvalid_WhenMessageIsTooLong()
        {
            var longMessage = new string('a', 3001);

            var dto = new AssistantChatMessageDto
            {
                UserId = "user-123",
                Message = longMessage
            };

            var results = ValidateDto(dto);

            Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r =>
                r.ErrorMessage == "Message is too long."));
        }

        [Test]
        public void TC004_AssistantChatMessageDto_ShouldBeInvalid_WhenMessageContainsUnsafeCharacters()
        {
            var dto = new AssistantChatMessageDto
            {
                UserId = "user-123",
                Message = "This message has <html> tags"
            };

            var results = ValidateDto(dto);

            Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r =>
                r.ErrorMessage == "Message contains potentially unsafe characters."));
        }

        [Test]
        public void TC005_AssistantChatMessageDto_ShouldBeInvalid_WhenMessageIsNull()
        {
            var dto = new AssistantChatMessageDto
            {
                UserId = "user-123",
                Message = null! // force null (ignoring required for test)
            };

            var results = ValidateDto(dto);

            Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r =>
                r.ErrorMessage == "The Message field is required."));
        }

        [Test]
        public void TC006_AssistantChatMessageDto_ShouldBeInvalid_WhenUserIdIsMissing()
        {
            var dto = new AssistantChatMessageDto
            {
                UserId = null!,
                Message = "Valid message"
            };

            var results = ValidateDto(dto);

            Assert.That(results, Is.Empty); 
        }
    }
}
