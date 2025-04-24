namespace NaviriaAPI.IServices.ISecurityService
{
    public interface IMessageSecurityService
    {
        void Validate(string userId, string message);
    }
}
