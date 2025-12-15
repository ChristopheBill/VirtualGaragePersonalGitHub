using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Contracts;

namespace VirtualGarage.Domain.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<UserResponseContract> CreateUserAsync(UserRequestContract userToCreate)
        {
            var userToCreateEntity = userToCreate.ToEntity();
            var createdUser = await userRepository.CreateUserAsync(userToCreateEntity);
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
        public async Task UpdateUserAsync(User user)
        {
            await userRepository.UpdateUserAsync(user);
        }
        public async Task DeleteUserAsync(Guid id)
        {
            await userRepository.DeleteUserAsync(id);
        }
    }
}
