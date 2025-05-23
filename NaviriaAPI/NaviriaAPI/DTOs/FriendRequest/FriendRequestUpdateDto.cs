using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.DTOs.FriendRequest
{
    public class FriendRequestUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
