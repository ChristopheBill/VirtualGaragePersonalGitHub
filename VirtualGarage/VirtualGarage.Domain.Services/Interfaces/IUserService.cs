using System;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Contracts;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseContract> CreateUserAsync(UserRequestContract user);
    Task<UserResponseContract?> GetUserByIdAsync(Guid id);
    Task<List<UserResponseContract?>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
}
