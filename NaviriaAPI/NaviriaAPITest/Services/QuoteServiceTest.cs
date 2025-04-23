using Moq;
using NUnit.Framework;
using NaviriaAPI.Services;
using NaviriaAPI.IRepositories;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class QuoteServiceTest
    {
        private Mock<IQuoteRepository> _mockQuoteRepository = null!;
        private QuoteService _quoteService = null!;

        [SetUp]
        public void SetUp()
        {
            _mockQuoteRepository = new Mock<IQuoteRepository>();
            _quoteService = new QuoteService(_mockQuoteRepository.Object);
        }

        [Test]
        public async Task TC01_CreateQuote_WithValidData_ReturnsQuoteDto()
        {
            var createDto = new QuoteCreateDto
            {
                Text = "Success is not final, failure is not fatal: It is the courage to continue that counts.",
                Language = "English"
            };

            QuoteEntity createdEntity = new()
            {
                Id = "1",
                Text = createDto.Text,
                Language = createDto.Language
            };

            _mockQuoteRepository.Setup(r => r.CreateAsync(It.IsAny<QuoteEntity>())).Returns(Task.CompletedTask);

            var result = await _quoteService.CreateAsync(createDto);

            Assert.That(result.Text, Is.EqualTo(createDto.Text));
            Assert.That(result.Language, Is.EqualTo(createDto.Language));
        }

        [Test]
        public async Task TC02_UpdateQuote_WithValidData_ReturnsTrue()
        {
            var updateDto = new QuoteUpdateDto
            {
                Text = "The only limit to our realization of tomorrow is our doubts of today.",
                Language = "English"
            };

            _mockQuoteRepository
                .Setup(r => r.UpdateAsync(It.IsAny<QuoteEntity>()))
                .ReturnsAsync(true);

            var result = await _quoteService.UpdateAsync("1", updateDto);

            Assert.That(result, Is.True);

        }

        [Test]
        public async Task TC03_GetQuote_ByValidId_ReturnsQuoteDto()
        {
            var entity = new QuoteEntity
            {
                Id = "1",
                Text = "Success is not final, failure is not fatal: It is the courage to continue that counts.",
                Language = "English"
            };

            _mockQuoteRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(entity);

            var result = await _quoteService.GetByIdAsync("1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo("1"));
            Assert.That(result.Text, Is.EqualTo("Success is not final, failure is not fatal: It is the courage to continue that counts."));
            Assert.That(result.Language, Is.EqualTo("English"));
        }


        [Test]
        public async Task TC04_GetAllQuotes_ReturnsListOfQuoteDto()
        {
            var entities = new List<QuoteEntity>
            {
                new() { Id = "1", Text = "Success is not final...", Language = "English" },
                new() { Id = "2", Text = "The only limit...", Language = "English" }
            };

            _mockQuoteRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            var result = (await _quoteService.GetAllAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task TC05_CreateQuote_WithInvalidData_ReturnsEmptyDto()
        {
            var createDto = new QuoteCreateDto
            {
                Text = "",
                Language = ""
            };

            _mockQuoteRepository.Setup(r => r.CreateAsync(It.IsAny<QuoteEntity>())).Returns(Task.CompletedTask);

            var result = await _quoteService.CreateAsync(createDto);

            Assert.That(result.Text, Is.EqualTo(""));
            Assert.That(result.Language, Is.EqualTo(""));
        }

        [Test]
        public async Task TC06_UpdateQuote_WithNonExistingId_ReturnsFalse()
        {
            var updateDto = new QuoteUpdateDto
            {
                Text = "New quote text",
                Language = "English"
            };

            _mockQuoteRepository
                .Setup(r => r.UpdateAsync(It.IsAny<QuoteEntity>()))
                .ReturnsAsync(false);

            var result = await _quoteService.UpdateAsync("9999", updateDto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC07_GetQuote_ByNonExistingId_ReturnsNull()
        {
            _mockQuoteRepository.Setup(r => r.GetByIdAsync("9999")).ReturnsAsync((QuoteEntity?)null);

            var result = await _quoteService.GetByIdAsync("9999");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC08_DeleteQuote_WithNonExistingId_ReturnsFalse()
        {
            _mockQuoteRepository.Setup(r => r.DeleteAsync("9999")).ReturnsAsync(false);

            var result = await _quoteService.DeleteAsync("9999");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC09_GetAllQuotes_WhenNoneExist_ReturnsEmptyList()
        {
            _mockQuoteRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<QuoteEntity>());

            var result = await _quoteService.GetAllAsync();

            Assert.That(result, Is.Empty);
        }
    }
}
