using NaviriaAPI.DTOs.FriendRequest;
using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.DTOs.FeaturesDTOs
{
    public class FriendRequestWithUserDto
    {
        public FriendRequestDto Request { get; set; } = null!;
        public UserDto Sender { get; set; } = null!;
    }
}
