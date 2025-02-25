using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.DTOs.UdateDTOs
{
    public class FriendRequestUpdateDto
    {
        public string FromUserId { get; set; } = string.Empty;
        public string ToUserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
