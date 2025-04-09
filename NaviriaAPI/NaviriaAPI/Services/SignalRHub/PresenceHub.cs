using Microsoft.AspNetCore.SignalR;
using NaviriaAPI.IRepositories;
using System.Security.Claims;

namespace NaviriaAPI.Services.SignalRHub
{
    public class PresenceHub : Hub
    {
        private readonly IUserRepository _userRepository;

        public PresenceHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await _userRepository.UpdatePresenceAsync(userId, DateTime.UtcNow, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await _userRepository.UpdatePresenceAsync(userId, DateTime.UtcNow, false);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
