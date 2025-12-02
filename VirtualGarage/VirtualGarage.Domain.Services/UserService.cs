using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Domain.Services
{
    public class UserService(IUserRepository userRepository)
    {
        public UserResponseContract CreateUser (User user)
        {
            var createdUser = userRepository.CreateUser(user);
            return ToContract(createdUser);
        }
    }
}
