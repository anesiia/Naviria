using NaviriaAPI.DTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Mappings;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Services.User
{
    public class UserSearchService : IUserSearchService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public UserSearchService(
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetUsersByTaskCategoryAsync(string categoryId)
        {
            var userIds = await _taskRepository.GetUserIdsByCategoryAsync(categoryId);

            if (userIds == null || !userIds.Any())
                throw new NotFoundException("No users found for this category.");

            var usersByCategory = await _userRepository.GetManyByIdsAsync(userIds);

            var userDtos = usersByCategory.Select(UserMapper.ToDto).ToList();

            return userDtos;
        }
    }
}
