using Microsoft.EntityFrameworkCore;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence
{
    public class UserRepository (DbContexts.VirtualGarageDbContext dbContext)
    {
        public User CreateUser (User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }
    }
}
