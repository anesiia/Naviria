using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IEmbeddedServices;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.Notification;
using Microsoft.Extensions.Logging;

namespace NaviriaAPI.Services.EmbeddedServices
{
    public class SupportService : ISupportService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SupportService> _logger;

        public SupportService(
            IQuoteRepository quoteRepository,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            ILogger<SupportService> logger)
        {
            _quoteRepository = quoteRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task SendSupportAsync(string senderUserId, string receiverUserId)
        {
            if (string.IsNullOrWhiteSpace(senderUserId) || string.IsNullOrWhiteSpace(receiverUserId))
            {
                _logger.LogWarning("Invalid sender or receiver user ID.");
                throw new ArgumentException("User IDs cannot be null or empty.");
            }

            var sender = await _userRepository.GetByIdAsync(senderUserId);
            var receiver = await _userRepository.GetByIdAsync(receiverUserId);
            if (sender == null || receiver == null)
            {
                _logger.LogWarning("Sender or receiver user not found.");
                throw new ArgumentException("One or both users not found.");
            }
            
            if (await HasSentSupportRecentlyAsync(sender.Nickname, receiverUserId))
                throw new InvalidOperationException("You can only send support to this user once every 24 hours.");

            var quotes = await _quoteRepository.GetAllAsync();
            if (!quotes.Any())
            {
                _logger.LogWarning("No quotes found in the database.");
                throw new InvalidOperationException("No quotes available to send.");
            }

            var random = new Random();
            var randomQuote = quotes.ElementAt(random.Next(quotes.Count()));

            var message = $"{randomQuote.Text} \n\n— Підтримка від {sender.Nickname}";

            var notification = new NotificationCreateDto
            {
                UserId = receiverUserId,
                Text = message,
                RecievedAt = DateTime.UtcNow
            };

            var entity = NotificationMapper.ToEntity(notification);
            await _notificationRepository.CreateAsync(entity);

            _logger.LogInformation("Support sent from {SenderId} to {ReceiverId}.", senderUserId, receiverUserId);
        }
        
        private async Task<bool> HasSentSupportRecentlyAsync(string senderNickname, string receiverUserId)
        {
            var recentThreshold = DateTime.UtcNow.AddHours(-24);
            var allReceiverNotifications = await _notificationRepository.GetAllByUserAsync(receiverUserId);

            return allReceiverNotifications.Any(n =>
                n.Text != null &&
                n.Text.Contains(senderNickname) &&
                n.RecievedAt >= recentThreshold);
        }
    }
}
