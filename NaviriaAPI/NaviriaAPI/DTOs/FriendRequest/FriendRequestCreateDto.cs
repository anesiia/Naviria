using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.DTOs.CreateDTOs
{
    public class FriendRequestCreateDto
    {
        public string FromUserId { get; set; } = string.Empty;

        public string ToUserId { get; set; } = string.Empty;
    }
}
