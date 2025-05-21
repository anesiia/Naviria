using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICloudStorage;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.helper
{
    // Testable Cloudinary Service для мокання
    public class TestableCloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IUserRepository _userRepository;
        private readonly ImageUploadResult _uploadResult;

        public TestableCloudinaryService(Cloudinary cloudinary, IUserRepository userRepository, ImageUploadResult uploadResult)
        {
            _cloudinary = cloudinary;
            _userRepository = userRepository;
            _uploadResult = uploadResult;
        }

        public async Task<bool> UploadImageAsync(string userId, IFormFile file)
        {
            // Мокаємо виклик Cloudinary
            var uploadResult = _uploadResult;

            if (uploadResult.StatusCode == HttpStatusCode.OK)
            {
                string imageUrl = uploadResult.SecureUrl.ToString();
                return await _userRepository.UpdateProfileImageAsync(userId, imageUrl);
            }

            throw new InvalidOperationException("Image upload failed.");
        }
    }
}
