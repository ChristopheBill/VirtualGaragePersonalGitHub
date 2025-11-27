using Microsoft.EntityFrameworkCore;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence
{
    public class UserRepository(DbContexts.VirtualGarageDbContext dbContext)
    {
        public User CreateUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user;
        }
        public User? GetUserById(int id)
        {
            return dbContext.Users
                .Include(u => u.Vehicles)
                .FirstOrDefault(u => u.Id == id);
        }
        public List<User> GetAllUsers()
        {
            return dbContext.Users
                .Include(u => u.Vehicles)
                .ToList();
        }
        public void UpdateUser(User user)
        {
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }
        public void DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }

        }
    }
}
