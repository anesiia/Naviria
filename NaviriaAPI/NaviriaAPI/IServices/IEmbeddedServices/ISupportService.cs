namespace NaviriaAPI.IServices.IEmbeddedServices;

public interface ISupportService
{
    Task SendSupportAsync(string senderUserId, string receiverUserId);
}