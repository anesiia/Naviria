using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs.FeaturesDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Entities.EmbeddedEntities;
using NaviriaAPI.IServices;
using NaviriaAPI.Exceptions;

namespace NaviriaAPI.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserService _userService;
        private readonly ILogger<FriendRequestService> _logger;
        private readonly IUserRepository _userRepository;

        public FriendRequestService(
            IFriendRequestRepository friendRequestRepository,
            IUserService userService,
            ILogger<FriendRequestService> logger,
            IUserRepository userRepository)
        {
            _friendRequestRepository = friendRequestRepository;
            _userService = userService;
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<FriendRequestDto> CreateAsync(FriendRequestCreateDto friendRequestDto)
        {
            if(friendRequestDto.ToUserId == friendRequestDto.FromUserId)
                throw new FailedToCreateException("Failed to create friend request. You can`t be a friend to yourself.");

            var entity = FriendRequestMapper.ToEntity(friendRequestDto);
            await _friendRequestRepository.CreateAsync(entity);
            return FriendRequestMapper.ToDto(entity);
        }
        public async Task<bool> UpdateAsync(string id, FriendRequestUpdateDto friendRequestDto)
        {

            var entity = await _friendRequestRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("Friend request is not found");

            entity.Status = friendRequestDto.Status;

            var updated = await _friendRequestRepository.UpdateAsync(entity);
            if (!updated)
                throw new FailedToUpdateException("Failed to update friend request");

            try
            {
                if (friendRequestDto.Status == "accepted")
                {
                    await AddToFriendsAsync(entity.FromUserId, entity.ToUserId);
                    await DeleteAsync(id);
                }
                else if (friendRequestDto.Status == "rejected")
                {
                    await DeleteAsync(id);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing friend request updating");
                return false;
            }
        }


        public async Task<bool> AddToFriendsAsync(string fromUserId, string toUserId)
        {
            var fromUser = await _userService.GetUserOrThrowAsync(fromUserId);
            var toUser = await _userService.GetUserOrThrowAsync(toUserId);

            if (fromUser.Friends.Any(f => f.UserId == toUserId) ||
                toUser.Friends.Any(f => f.UserId == fromUserId))
            {
                throw new AlreadyExistException("Failed to add friends. These users are already friends");
            }

            fromUser.Friends.Add(new UserFriendInfo
            {
                UserId = toUser.Id,
                Nickname = toUser.Nickname,
            });

            toUser.Friends.Add(new UserFriendInfo
            {
                UserId = fromUser.Id,
                Nickname = fromUser.Nickname,
            });

            try
            {
                var firstUser = await _userRepository.UpdateAsync(fromUser);
                var secondUser = await _userRepository.UpdateAsync(toUser);

                return firstUser && secondUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding users {From} and {To} to friends", fromUserId, toUserId);
                return false;
            }
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

        public async Task<IEnumerable<FriendRequestWithUserDto>> GetIncomingRequestsAsync(string toUserId)
        {
            var requests = await _friendRequestRepository.GetByReceiverIdAsync(toUserId);
            var senderIds = requests.Select(r => r.FromUserId).Distinct().ToList();
            var senders = await _userRepository.GetManyByIdsAsync(senderIds);

            var senderDict = senders.ToDictionary(u => u.Id);

            var result = requests
                .Where(r => senderDict.ContainsKey(r.FromUserId))
                .Select(r => new FriendRequestWithUserDto
                {
                    Request = FriendRequestMapper.ToDto(r),
                    Sender = UserMapper.ToDto(senderDict[r.FromUserId])
                });

            return result;
        }


    }
}
