using MongoDB.Driver;
using NaviriaAPI.Data;
using NaviriaAPI.Entities;
using NaviriaAPI.Repositories;
using NUnit.Framework;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using NaviriaAPI.IRepositories;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace NaviriaAPITest.RepositoriesTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IMongoDbContext _dbContext;
        private IUserRepository _userRepository;
        private IMongoCollection<UserEntity> _usersCollection;

        [SetUp]
        public void SetUp()
        {
            // Зчитуємо налаштування з файлу
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("MongoDbSettings.json")
                .Build();

            var mongoDbSettings = configuration.Get<MongoDbSettings>(); // Отримуємо налаштування

            // Створюємо IOptions для передачі в MongoDbContext
            var mockOptions = new Mock<IOptions<MongoDbSettings>>();
            mockOptions.Setup(o => o.Value).Returns(mongoDbSettings);

            // Створення контексту та репозиторію з моком
            _dbContext = new MongoDbContext(mockOptions.Object);
            _userRepository = new UserRepository(_dbContext);

            // Очищення колекції користувачів
            _usersCollection = _dbContext.Users;
            _usersCollection.DeleteMany(FilterDefinition<UserEntity>.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            // Очищення колекції після тесту
            if (_userRepository != null)
            {
                _userRepository = null;
            }
        }

        [Test]
        public async Task TC001_CreateAsync_And_GetByIdAsync_ShouldWorkCorrectly()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "test@example.com",
                Nickname = "TestUser",
                LastSeen = DateTime.UtcNow,
                IsOnline = true
            };

            // Act
            await _userRepository.CreateAsync(user);
            var fetchedUser = await _userRepository.GetByIdAsync(user.Id);

            // Assert
            Assert.That(fetchedUser, Is.Not.Null); 
            Assert.That(fetchedUser.Email, Is.EqualTo(user.Email));
            Assert.That(fetchedUser.Nickname, Is.EqualTo(user.Nickname));
        }

        [Test]
        public async Task TC002_GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var user1 = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "user1@example.com",
                Nickname = "user1",
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                Photo = "https://example.com/photo1.jpg"
            };

            var user2 = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "user2@example.com",
                Nickname = "user2",
                LastSeen = DateTime.UtcNow,
                IsOnline = false,
                Photo = "https://example.com/photo2.jpg"
            };

            await _userRepository.CreateAsync(user1);
            await _userRepository.CreateAsync(user2);

            // Act
            var users = await _userRepository.GetAllAsync();

            // Assert
            Assert.That(users.Count, Is.EqualTo(2));
            Assert.That(users[0].Email, Is.EqualTo("user1@example.com"));
            Assert.That(users[1].Email, Is.EqualTo("user2@example.com"));
        }

        [Test]
        public async Task TC003_GetUserById_ShouldReturnUser()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "user@example.com",
                Nickname = "user",
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);

            // Act
            var fetchedUser = await _userRepository.GetByIdAsync(user.Id);

            // Assert
            Assert.That(fetchedUser, Is.Not.Null);
            Assert.That(fetchedUser.Id, Is.EqualTo(user.Id));
        }

        [Test]
        public async Task TC004_UpdateUser_ShouldUpdateUser()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "user@example.com",
                Nickname = "user",
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);
            user.Nickname = "updateduser"; // Update nickname

            // Act
            var result = await _userRepository.UpdateAsync(user);

            // Assert
            var updatedUser = await _userRepository.GetByIdAsync(user.Id);
            Assert.That(result, Is.True);
            Assert.That(updatedUser.Nickname, Is.EqualTo("updateduser"));
        }

        [Test]
        public async Task TC005_DeleteUser_ShouldDeleteUser()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "delete@example.com",
                Nickname = "deleteuser",
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);

            // Act
            var result = await _userRepository.DeleteAsync(user.Id);

            // Assert
            var deletedUser = await _userRepository.GetByIdAsync(user.Id);
            Assert.That(result, Is.True);
            Assert.That(deletedUser, Is.Null);
        }

        [Test]
        public async Task TC006_GetUserByEmail_ShouldReturnUser()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "emailuser@example.com",
                Nickname = "emailuser",
                LastSeen = DateTime.UtcNow,
                IsOnline = false,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);

            // Act
            var fetchedUser = await _userRepository.GetByEmailAsync("emailuser@example.com");

            // Assert
            Assert.That(fetchedUser, Is.Not.Null);
            Assert.That(fetchedUser.Email, Is.EqualTo("emailuser@example.com"));
        }

        [Test]
        public async Task TC007_GetUserByNickname_ShouldReturnUser()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "nicknameuser@example.com",
                Nickname = "nicknameuser",
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);

            // Act
            var fetchedUser = await _userRepository.GetByNicknameAsync("nicknameuser");

            // Assert
            Assert.That(fetchedUser, Is.Not.Null);
            Assert.That(fetchedUser.Nickname, Is.EqualTo("nicknameuser"));
        }

        [Test]
        public async Task TC008_UpdatePresence_ShouldUpdatePresence()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "presenceuser@example.com",
                Nickname = "presenceuser",
                LastSeen = DateTime.UtcNow,
                IsOnline = false,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);
            var newLastSeen = DateTime.UtcNow.AddHours(1);
            var isOnline = true;

            // Act
            var result = await _userRepository.UpdatePresenceAsync(user.Id, newLastSeen, isOnline);

            // Assert
            var updatedUser = await _userRepository.GetByIdAsync(user.Id);
            Assert.That(result, Is.True);
            Assert.That(updatedUser.LastSeen, Is.EqualTo(newLastSeen).Within(TimeSpan.FromSeconds(1)));
            Assert.That(updatedUser.IsOnline, Is.True);
        }

        [Test]
        public async Task TC009_UpdateProfileImage_ShouldUpdateProfileImage()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Email = "profileimageuser@example.com",
                Nickname = "profileimageuser",
                LastSeen = DateTime.UtcNow,
                IsOnline = true,
                Photo = "https://example.com/photo.jpg"
            };

            await _userRepository.CreateAsync(user);
            var newImageUrl = "https://example.com/newphoto.jpg";

            // Act
            var result = await _userRepository.UpdateProfileImageAsync(user.Id, newImageUrl);

            // Assert
            var updatedUser = await _userRepository.GetByIdAsync(user.Id);
            Assert.That(result, Is.True);
            Assert.That(updatedUser.Photo, Is.EqualTo(newImageUrl));
        }
    }
}



























//using MongoDB.Driver;
//using NaviriaAPI.Data;
//using NaviriaAPI.Entities;
//using NaviriaAPI.Repositories;
//using NUnit.Framework;
//using Microsoft.Extensions.Options;
//using Moq;
//using System;
//using System.Threading.Tasks;
//using NaviriaAPI.IRepositories;
//using MongoDB.Bson;

//namespace NaviriaAPI.Tests
//{
//    [TestFixture]
//    public class UserRepositoryTests
//    {
//        private IMongoDbContext _dbContext;
//        private IUserRepository _userRepository;
//        private IMongoCollection<UserEntity> _usersCollection;

//        [SetUp]
//        public void SetUp()
//        {
//            // Мок для MongoDbSettings
//            var mongoDbSettings = new MongoDbSettings
//            {
//                ConnectionString = "mongodb://localhost:27017",
//                DatabaseName = "NaviriaTestDB"
//            };

//            var mockOptions = new Mock<IOptions<MongoDbSettings>>();
//            mockOptions.Setup(x => x.Value).Returns(mongoDbSettings);

//            // Створення контексту та репозиторію з моком
//            _dbContext = new MongoDbContext(mockOptions.Object);
//            _userRepository = new UserRepository(_dbContext);
//        }

//        [Test]
//        public async Task CreateUser_ShouldInsertUser()
//        {
//            // Arrange
//            var user = new UserEntity
//            {
//                Id = ObjectId.GenerateNewId().ToString(),
//                Email = "test@example.com",
//                Nickname = "testuser",
//                LastSeen = DateTime.UtcNow,
//                IsOnline = true,
//                Photo = "https://res.cloudinary.com/dyvnoao0d/image/upload/v1744484518/users_photos/b4eeee78-b746-4a7c-828c-fd3a64317f2d.jpg"
//            };

//            // Act
//            await _userRepository.CreateAsync(user);

//            // Assert
//            var insertedUser = await _userRepository.GetByIdAsync(user.Id);
//            Assert.That(insertedUser, Is.Not.Null);
//            Assert.That(insertedUser.Email, Is.EqualTo("test@example.com"));
//            Assert.That(insertedUser.Nickname, Is.EqualTo("testuser"));
//        }

//    }
//}
