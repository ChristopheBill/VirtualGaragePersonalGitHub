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
        public User CreateUser(User user);
        public User? GetUserById(Guid id);
        public List<User> GetAllUsers();
        public void UpdateUser(User user);
        public void DeleteUser(Guid id);
    }
}
