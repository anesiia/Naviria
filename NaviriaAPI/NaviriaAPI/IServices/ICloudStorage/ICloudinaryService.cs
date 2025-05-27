namespace NaviriaAPI.IServices.ICloudStorage
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAndGetUrlAsync(IFormFile file);
    }
}
