namespace NaviriaAPI.IServices
{
    public interface IUserCleanupService
    {
        Task<bool> DeleteUserAndRelatedDataAsync(string userId);
    }

}
