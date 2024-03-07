using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetAllAsync(FilterUser filterUser);
        Task<User?> GetByIdAsync(Guid id);
        Task<User> AddUserAsync(User user);
    }
}
