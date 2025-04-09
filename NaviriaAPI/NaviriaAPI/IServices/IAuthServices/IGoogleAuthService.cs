namespace NaviriaAPI.IServices.IAuthServices
{
    public interface IGoogleAuthService
    {
        Task<string> AuthenticateAsync(string idToken);
    }
}
