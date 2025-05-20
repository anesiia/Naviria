namespace NaviriaAPI.IServices.IUserServices
{
    public interface IUserCleanupService
    {
        Task<bool> DeleteUserAndRelatedDataAsync(string userId);
    }

}
