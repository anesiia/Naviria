using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviriaAPI.DTOs.UpdateDTOs
{
    public class FriendRequestUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
