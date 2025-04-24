using NUnit.Framework;
using NaviriaAPI.DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.Tests.DTOs.CreateDTOs
{
    [TestFixture]
    public class NotificationCreateDtoTests
    {
        private static bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Test]
        public void TC001_NotificationCreateDto_ValidData_ShouldPassValidation()
        {
            var dto = new NotificationCreateDto
            {
                UserId = "user123",
                Text = "This is a valid notification.",
                RecievedAt = DateTime.UtcNow
            };

            var isValid = ValidateModel(dto, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void TC002_NotificationCreateDto_EmptyUserId_ShouldFailValidation()
        {
            var dto = new NotificationCreateDto
            {
                UserId = "",
                Text = "Still a valid message",
                RecievedAt = DateTime.UtcNow
            };

            var isValid = ValidateModel(dto, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results, Has.Some.Matches<ValidationResult>(r => r.MemberNames.Contains("UserId")));
        }

        [Test]
        public void TC003_NotificationCreateDto_TextTooLong_ShouldFailValidation()
        {
            var longText = new string('A', 151); // exceeds 150 chars
            var dto = new NotificationCreateDto
            {
                UserId = "user123",
                Text = longText,
                RecievedAt = DateTime.UtcNow
            };

            var isValid = ValidateModel(dto, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results, Has.Some.Matches<ValidationResult>(r => r.MemberNames.Contains("Text")));
        }

       
    }
}