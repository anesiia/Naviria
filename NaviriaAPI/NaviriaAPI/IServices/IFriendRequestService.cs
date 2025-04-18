using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using System;

public interface IFriendRequestService
{
    Task<IEnumerable<FriendRequestDto>> GetAllAsync();
    Task<FriendRequestDto?> GetByIdAsync(string id);
    Task<FriendRequestDto> CreateAsync(FriendRequestCreateDto friendRequestDto);
    Task<bool> UpdateAsync(string id, FriendRequestUpdateDto friendRequestDto);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<FriendRequestDto>> GetIncomingRequestsAsync(string toUserId);

}
