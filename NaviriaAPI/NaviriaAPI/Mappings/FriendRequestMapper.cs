using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using SharpCompress.Common;

namespace NaviriaAPI.Mappings
{
    public class FriendRequestMapper
    {
        // Entity → DTO (для відправки в API)
        public static FriendRequestDto ToDto(FriendRequestEntity entity) =>
            new FriendRequestDto { Id = entity.Id, FromUserId = entity.FromUserId, ToUserId = entity.ToUserId, Status = entity.Status };

        // 🟩 DTO → Entity (для збереження в БД)
        public static FriendRequestEntity ToEntity(FriendRequestDto dto) =>
            new FriendRequestEntity { Id = dto.Id, FromUserId = dto.FromUserId, ToUserId = dto.ToUserId, Status = dto.Status };

        // CreateDto → Entity (для створення)
        public static FriendRequestEntity ToEntity(FriendRequestCreateDto dto) =>
            new FriendRequestEntity { FromUserId = dto.FromUserId, ToUserId = dto.ToUserId, Status = "new request" };

        // UpdateDto → Entity (для оновлення)
        public static FriendRequestEntity ToEntity(string id, FriendRequestUpdateDto dto) =>
            new FriendRequestEntity { Id = id, Status = dto.Status};
    }
}
