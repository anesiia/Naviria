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
        private readonly IUserRepository _userRepository;
        private readonly ImageUploadResult _uploadResult;

        public TestableCloudinaryService(IUserRepository userRepository, ImageUploadResult uploadResult)
        {
            _userRepository = userRepository;
            _uploadResult = uploadResult;
        }

        public async Task<string> UploadImageAndGetUrlAsync(IFormFile file)
        {
            if (_uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return _uploadResult.SecureUrl.ToString();
            }

            throw new InvalidOperationException("Image upload failed.");
        }

        public async Task<bool> UploadImageAsync(string userId, IFormFile file)
        {
            string imageUrl = await UploadImageAndGetUrlAsync(file);
            return await _userRepository.UpdateProfileImageAsync(userId, imageUrl);
        }
    }
}