using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(User user);
        public Task<User?> GetUserByIdAsync(Guid id);
        public Task<List<User>> GetAllUsersAsync();
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(Guid id);
    }
}
