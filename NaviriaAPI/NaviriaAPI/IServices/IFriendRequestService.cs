using NaviriaAPI.DTOs.CreateDTOs;
using System;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.DTOs.FriendRequest;

namespace NaviriaAPI.IServices
{
    public interface IFriendRequestService
    {
        Task<IEnumerable<FriendRequestDto>> GetAllAsync();
        Task<FriendRequestDto?> GetByIdAsync(string id);
        Task<FriendRequestDto> CreateAsync(FriendRequestCreateDto friendRequestDto);
        Task<bool> UpdateAsync(string id, FriendRequestUpdateDto friendRequestDto);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<FriendRequestWithUserDto>> GetIncomingRequestsAsync(string toUserId);

    }
}

