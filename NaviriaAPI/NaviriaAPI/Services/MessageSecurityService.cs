using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices.ISecurityService;
using System.Text.RegularExpressions;

namespace NaviriaAPI.Services
{
    public class MessageSecurityService : IMessageSecurityService
    {
        private readonly ILogger<MessageSecurityService> _logger;

        private static readonly string[] DangerousKeywords = new[]
        {
        "script", "alert", "onerror", "onload", "iframe", "src=",
        "SELECT", "INSERT", "DELETE", "DROP", "UPDATE", "--", ";", "/*", "*/", "OR 1=1", "UNION", "xp_cmdshell"
        };

        private static readonly Regex DangerousPattern = new Regex(
            @"(\b(SELECT|INSERT|DELETE|DROP|UPDATE|UNION|XP_CMDSHELL)\b|<script|</script>|--|\*\/|\/\*|OR\s+1=1)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        public MessageSecurityService(ILogger<MessageSecurityService> logger)
        {
            _logger = logger;
        }

        public void Validate(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var matchedKeyword = DangerousKeywords
                .FirstOrDefault(k => message.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (matchedKeyword != null)
            {
                LogAndThrow(userId, message, $"keyword match: {matchedKeyword}");
            }

            if (DangerousPattern.IsMatch(message))
            {
                LogAndThrow(userId, message, "regex match");
            }
        }

        private void LogAndThrow(string userId, string message, string reason)
        {
            _logger.LogWarning("🛡 Suspicious message blocked from user {UserId}. Reason: {Reason}. Message: {Message}",
                userId, reason, message);

            throw new SuspiciousMessageException("Your message contains suspicious content and was blocked.");
        }
    }

}
