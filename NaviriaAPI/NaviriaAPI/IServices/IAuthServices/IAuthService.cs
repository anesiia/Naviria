namespace NaviriaAPI.IServices.IAuthServices
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string email, string password);
    }
}
