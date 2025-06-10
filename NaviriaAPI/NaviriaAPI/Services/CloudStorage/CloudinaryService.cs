using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.ICloudStorage;

namespace NaviriaAPI.Services.CloudStorage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        //public async Task<string> UploadImageAndGetUrlAsync(IFormFile file)
        //{
        //    using var stream = file.OpenReadStream();
        //    var uploadParams = new ImageUploadParams
        //    {
        //        File = new FileDescription(file.FileName, stream),
        //        Folder = "users_photos",
        //        PublicId = Guid.NewGuid().ToString(),
        //        UseFilename = true,
        //        UniqueFilename = true,
        //        Overwrite = false
        //    };

        //    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        //    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        //        return uploadResult.SecureUrl.ToString();

        //    throw new InvalidOperationException("Image upload failed.");
        //}
        public async Task<string> UploadImageAndGetUrlAsync(IFormFile file)
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
                return uploadResult.SecureUrl.ToString();

            // Выводим в лог причину ошибки и кидаем подробное исключение
            var errorMsg = uploadResult.Error != null ? uploadResult.Error.Message : "Unknown error";
            throw new InvalidOperationException($"Image upload failed. Cloudinary error: {errorMsg}");
        }
    }
}
