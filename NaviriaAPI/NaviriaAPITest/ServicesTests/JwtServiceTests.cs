using NUnit.Framework;
using NaviriaAPI.Services.JwtTokenService;
using NaviriaAPI.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace NaviriaAPITest.ServicesTests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private JwtService _jwtService;
        private string _secret;

        [SetUp]
        public void SetUp()
        {
            _secret = "supersecretkey1234567890_SUPERSTRONGKEY";

            var configValues = new Dictionary<string, string>
            {
                {"Jwt:Secret", _secret},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues!)
                .Build();

            _jwtService = new JwtService(configuration);
        }

        [Test]
        public void TC01_GenerateUserToken_ProUser_ShouldContainProUserClaim()
        {
            var user = new UserEntity
            {
                Id = "123",
                Email = "pro@example.com",
                IsProUser = true
            };

            var token = _jwtService.GenerateUserToken(user);
            var principal = _jwtService.ValidateToken(token);

            Assert.That(token, Is.Not.Empty);
            Assert.That(principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, Is.EqualTo("123"));
            Assert.That(principal?.FindFirst(ClaimTypes.Email)?.Value, Is.EqualTo("pro@example.com"));
            Assert.That(principal?.FindFirst(ClaimTypes.Role)?.Value, Is.EqualTo("ProUser"));
        }

        [Test]
        public void TC02_GenerateUserToken_FreeUser_ShouldContainFreeUserClaim()
        {
            var user = new UserEntity
            {
                Id = "321",
                Email = "free@example.com",
                IsProUser = false
            };

            var token = _jwtService.GenerateUserToken(user);
            var principal = _jwtService.ValidateToken(token);

            Assert.That(token, Is.Not.Empty);
            Assert.That(principal?.FindFirst(ClaimTypes.Role)?.Value, Is.EqualTo("FreeUser"));
        }

        [Test]
        public void TC03_ValidateToken_InvalidToken_ShouldReturnNull()
        {
            var wrongKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wrongsecretkey_that_is_long_enough"));
            var creds = new SigningCredentials(wrongKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TestIssuer",
                audience: "TestAudience",
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Email, "wrong@example.com"),
                    new Claim(ClaimTypes.Role, "FreeUser")
                },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var result = _jwtService.ValidateToken(tokenString);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TC04_ValidateToken_ExpiredToken_ShouldReturnNull()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiredToken = new JwtSecurityToken(
                issuer: "TestIssuer",
                audience: "TestAudience",
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "999"),
                    new Claim(ClaimTypes.Email, "expired@example.com"),
                    new Claim(ClaimTypes.Role, "FreeUser")
                },
                expires: DateTime.UtcNow.AddMinutes(-10),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(expiredToken);
            var result = _jwtService.ValidateToken(tokenString);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TC05_GenerateUserToken_TokenShouldExpireIn7Days()
        {
            var user = new UserEntity
            {
                Id = "555",
                Email = "test@example.com",
                IsProUser = false
            };

            var tokenString = _jwtService.GenerateUserToken(user);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            var expectedExpiration = DateTime.UtcNow.AddDays(7);
            var actualExpiration = token.ValidTo;

            Assert.That(actualExpiration, Is.EqualTo(expectedExpiration).Within(TimeSpan.FromSeconds(10)));
        }

        [Test]
        public void TC06_ValidateToken_TokenWithWrongAudience_ShouldReturnNull()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TestIssuer",
                audience: "WrongAudience",
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "101"),
                    new Claim(ClaimTypes.Email, "wrongaud@example.com"),
                    new Claim(ClaimTypes.Role, "FreeUser")
                },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var result = _jwtService.ValidateToken(tokenString);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TC07_ValidateToken_TokenWithWrongIssuer_ShouldReturnNull()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "WrongIssuer",
                audience: "TestAudience",
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "202"),
                    new Claim(ClaimTypes.Email, "wrongiss@example.com"),
                    new Claim(ClaimTypes.Role, "ProUser")
                },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var result = _jwtService.ValidateToken(tokenString);

            Assert.That(result, Is.Null);
        }
    }
}

