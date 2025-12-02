using Microsoft.EntityFrameworkCore;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Persistence
{
    public class UserRepository(DbContexts.VirtualGarageDbContext dbContext) : IUserRepository
    {
        public async Task<User> CreateUserAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await dbContext.Users
                .Include(u => u.Vehicles)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await dbContext.Users
                .Include(u => u.Vehicles)
                .ToListAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(Guid id)
        { 
             var user = await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
