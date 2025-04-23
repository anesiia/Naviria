namespace NaviriaAPI.IServices
{
    public interface IAssistantChatService
    {
        Task<string> SendMessageAsync(string userId, string message);
    }
}
