using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;
using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.IServices.IUserServices
{
    /// <summary>
    /// Service for managing users, including registration, update, deletion, achievements, and queries.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users from the system.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="UserDto"/> objects.</returns>
        Task<IEnumerable<UserDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>The <see cref="UserDto"/> if found; otherwise, <c>null</c>.</returns>
        Task<UserDto?> GetByIdAsync(string id);

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="userDto">The data required to create the user.</param>
        /// <returns>
        /// The JWT token of the newly created user.
        /// Throws <see cref="EmailAlreadyExistException"/> if email is already used,
        /// or <see cref="NicknameAlreadyExistException"/> if nickname is already used.
        /// </returns>
        Task<string> CreateAsync(UserCreateDto userDto);

        /// <summary>
        /// Updates the user's data.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="userDto">The updated user data.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateAsync(string id, UserUpdateDto userDto);

        /// <summary>
        /// Deletes the user and all related data from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Grants a specific achievement to the user.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="achievementId">The achievement's unique identifier.</param>
        /// <returns>True if the achievement was granted; otherwise, false.</returns>
        Task<bool> GiveAchievementAsync(string userId, string achievementId);

        /// <summary>
        /// Retrieves the user entity by their ID or throws an exception if not found.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>The <see cref="UserEntity"/> if found.</returns>
        /// <exception cref="NotFoundException">Thrown if the user is not found.</exception>
        Task<UserEntity> GetUserOrThrowAsync(string id);

        /// <summary>
        /// Checks if a user with the specified ID exists.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        Task<bool> UserExistsAsync(string userId);
    }
}
