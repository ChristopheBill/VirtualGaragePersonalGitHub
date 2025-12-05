using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Domain.Services
{
    public class UserService(IUserRepository userRepository)
    {
        public Task<UserResponseContract> CreateUserAsync(User user)
        {
            var createdUser = userRepository.CreateUserAsync(user);
            return createdUser.ToContract();
        }
        public UserResponseContract? GetUserByIdAsync(Guid id)
        {
            var user = userRepository.GetUserByIdAsync(id);
            return user?.ToContract();
        }
        public List<UserResponseContract> GetAllUsersAsync()
        {
            var users = userRepository.GetAllUsersAsync();
            return users.ConvertAll(u => u.ToContract());
        }
        public UserResponseContract? UpdateUserAsync(Guid id, User user)
        {
            var updatedUser = userRepository.UpdateUserAsync(id, user);
            return updatedUser?.ToContract();
        }
        public bool DeleteUserAsync(Guid id)
        {
            return userRepository.DeleteUser(id);
        }
    }
}
