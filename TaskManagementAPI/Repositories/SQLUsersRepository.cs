using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public class SQLUsersRepository(TaskManagementDbContext dbContext) : IUsersRepository
    {
        private readonly TaskManagementDbContext dbContext = dbContext;

        public async Task<User> AddUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.DtCreatedDate = DateTime.Now;
            user.Name = user.Name.Trim().ToUpper();
            user.Email = user.Email.Trim().ToLower();
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync(FilterUser filterUser)
        {
            filterUser.Email = (string.IsNullOrWhiteSpace(filterUser.Email)? string.Empty : filterUser.Email).Trim().ToLower();
            return await dbContext.Users.Where(x => 
                (filterUser.UserId == Guid.Empty || x.Id == filterUser.UserId) &&
                (string.IsNullOrWhiteSpace(filterUser.Email) || x.Email == filterUser.Email)).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
