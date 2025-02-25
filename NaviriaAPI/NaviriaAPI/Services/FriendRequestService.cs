using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs.UdateDTOs;

namespace NaviriaAPI.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        public FriendRequestService(IFriendRequestRepository FriendRequestRepository)
        {
            _friendRequestRepository = FriendRequestRepository;
        }
        public async Task<FriendRequestDto> CreateAsync(FriendRequestCreateDto createDto)
        {
            var entity = FriendRequestMapper.ToEntity(createDto);
            await _friendRequestRepository.CreateAsync(entity);
            return FriendRequestMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, FriendRequestUpdateDto updateDto)
        {
            var entity = FriendRequestMapper.ToEntity(id, updateDto);
            return await _friendRequestRepository.UpdateAsync(entity);
        }
        public async Task<FriendRequestDto?> GetByIdAsync(string id)
        {
            var entity = await _friendRequestRepository.GetByIdAsync(id);
            return entity == null ? null : FriendRequestMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(string id) =>
            await _friendRequestRepository.DeleteAsync(id);

        public async Task<IEnumerable<FriendRequestDto>> GetAllAsync()
        {
            var categories = await _friendRequestRepository.GetAllAsync();
            return categories.Select(FriendRequestMapper.ToDto).ToList();
        }
    }
}
