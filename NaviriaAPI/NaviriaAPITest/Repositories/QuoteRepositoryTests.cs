using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.Repositories;
using NaviriaAPI.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Linq;
using NaviriaAPI.DTOs;
using NaviriaAPI.Options;
using NaviriaAPI.Tests.helper;

namespace NaviriaAPI.Tests.Repositories
{
    [TestFixture]
    public class QuoteRepositoryTests : RepositoryTestBase<QuoteEntity>
    {
        private IQuoteRepository _quoteRepository = null!;

        public override void SetUp()
        {
            base.SetUp();
            _quoteRepository = new QuoteRepository(DbContext);
        }

        protected override IMongoCollection<QuoteEntity> GetCollection(IMongoDbContext dbContext)
        {
            return dbContext.Quotes;
        }


        [Test]
        public async Task CreateAsync_ShouldInsertQuote()
        {
            // Arrange
            var newQuote = new QuoteEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = "This is a test quote",
                Language = "en"
            };

            // Act
            await _quoteRepository.CreateAsync(newQuote);

            // Assert
            var insertedQuote = await _quoteRepository.GetByIdAsync(newQuote.Id);
            Assert.That(insertedQuote, Is.Not.Null);
            Assert.That(insertedQuote?.Text, Is.EqualTo(newQuote.Text));
            Assert.That(insertedQuote?.Language, Is.EqualTo(newQuote.Language));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateQuote()
        {
            // Arrange
            var newQuote = new QuoteEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = "Original Quote",
                Language = "en"
            };
            await _quoteRepository.CreateAsync(newQuote);

            var updatedQuote = new QuoteEntity
            {
                Id = newQuote.Id,
                Text = "Updated Quote",
                Language = "en"
            };

            // Act
            var isUpdated = await _quoteRepository.UpdateAsync(updatedQuote);

            // Assert
            var retrievedQuote = await _quoteRepository.GetByIdAsync(newQuote.Id);
            Assert.That(isUpdated, Is.True);
            Assert.That(retrievedQuote?.Text, Is.EqualTo(updatedQuote.Text));
            Assert.That(retrievedQuote?.Language, Is.EqualTo(updatedQuote.Language));
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteQuote()
        {
            // Arrange
            var newQuote = new QuoteEntity
            {
                Id = ObjectId.GenerateNewId().ToString(), // це все ще string
                Text = "This is a quote to be deleted",
                Language = "en"
            };
            await _quoteRepository.CreateAsync(newQuote);

            // Act
            var isDeleted = await _quoteRepository.DeleteAsync(newQuote.Id);

            // Assert
            var deletedQuote = await _quoteRepository.GetByIdAsync(newQuote.Id);
            Assert.That(isDeleted, Is.True);
            Assert.That(deletedQuote, Is.Null); // ⬅️ важливо: має бути Null, бо запис видалено
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllQuotes()
        {
            // Arrange
            var quote1 = new QuoteEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = "Quote 1",
                Language = "en"
            };
            var quote2 = new QuoteEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = "Quote 2",
                Language = "en"
            };
            await _quoteRepository.CreateAsync(quote1);
            await _quoteRepository.CreateAsync(quote2);

            // Act
            var allQuotes = await _quoteRepository.GetAllAsync();

            // Assert
            Assert.That(allQuotes.Count, Is.EqualTo(2));
            Assert.That(allQuotes.Any(q => q.Text == "Quote 1"), Is.True);
            Assert.That(allQuotes.Any(q => q.Text == "Quote 2"), Is.True);
        }
    }
}