namespace NaviriaAPI.IServices.ICloudStorage
{
    public interface ICloudinaryService
    {
        Task<bool> UploadImageAsync(string userId, IFormFile file);
    }
}
