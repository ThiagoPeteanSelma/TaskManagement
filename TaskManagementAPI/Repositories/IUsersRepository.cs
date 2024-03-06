using TaskManagement.API.Models.Domain;

namespace TaskManagement.API.Repositories
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllAsync();
    }
}
