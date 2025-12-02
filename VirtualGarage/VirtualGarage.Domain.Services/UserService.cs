using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Domain.Services
{
    public class UserService(IUserRepository userRepository)
    {
        public UserResponseContract CreateUser(User user)
        {
            var createdUser = userRepository.CreateUser(user);
            return createdUser.ToContract();
        }
        public UserResponseContract? GetUserById(Guid id)
        {
            var user = userRepository.GetUserById(id);
            return user?.ToContract();
        }
        public List<UserResponseContract> GetAllUsers()
        {
            var users = userRepository.GetAllUsers();
            return users.ConvertAll(u => u.ToContract());
        }
        public UserResponseContract? UpdateUser(Guid id, User user)
        {
            var updatedUser = userRepository.UpdateUser(id, user);
            return updatedUser?.ToContract();
        }
        public bool DeleteUser(Guid id)
        {
            return userRepository.DeleteUser(id);
        }
    }
}
