using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Domain.Services
{
    public class UserService(IUserRepository userRepository)
    {
        public async Task<UserResponseContract> CreateUserAsync(User user)
        {
            var createdUser = await userRepository.CreateUserAsync(user);
            return createdUser.ToContract();
        }
        public async Task<UserResponseContract?> GetUserByIdAsync(Guid id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            return user?.ToContract();
        }
        public async Task<List<UserResponseContract?>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();
            var userContracts = users.Select(user => user.ToContract()).ToList();
            return userContracts;
        }
        public async Task <UserResponseContract?> UpdateUserAsync(User user)
        {
            var updatedUser = await userRepository.UpdateUserAsync(user);
            return updatedUser?.ToContract();
        }
        public async Task DeleteUserAsync(Guid id)
        {
            await userRepository.DeleteUserAsync(id);
        }
    }
}
