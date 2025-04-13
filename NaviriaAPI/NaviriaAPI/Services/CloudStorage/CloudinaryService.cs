using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPI.Services.CloudStorage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IUserRepository _userRepository;

        public CloudinaryService(Cloudinary cloudinary, IUserRepository userRepository)
        {
            _cloudinary = cloudinary;
            _userRepository = userRepository;
        }

        public async Task<bool> UploadImageAsync(string userId, IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "users_photos",
                PublicId = Guid.NewGuid().ToString(),
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string imageUrl = uploadResult.SecureUrl.ToString();
                return await _userRepository.UpdateProfileImageAsync(userId, imageUrl);
            }
           
            throw new InvalidOperationException("Image upload failed.");
        }
    }
}
