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
            var createdUser = userRepository.CreateUserAsync(user);
            return Task.FromResult(createdUser.ToContract());
        }
        public async Task<UserResponseContract?> GetUserByIdAsync(Guid id)
        {
            var user = userRepository.GetUserByIdAsync(id);
            return Task.FromResult(user?.ToContract());
        }
        public async Task<List<UserResponseContract?>> GetAllUsersAsync()
        {
            var users = userRepository.GetAllUsersAsync();
            var userContracts = users.Select(user => user.ToContract()).ToList();
            return Task.FromResult(userContracts);
        }
        public async Task <UserResponseContract?> UpdateUserAsync(User user)
        {
            var updatedUser = userRepository.UpdateUserAsync(user);
            return Task.FromResult(updatedUser?.ToContract());
        }
        public async Task DeleteUserAsync(Guid id)
        {
            await userRepository.DeleteUserAsync(id);
        }
    }
}
