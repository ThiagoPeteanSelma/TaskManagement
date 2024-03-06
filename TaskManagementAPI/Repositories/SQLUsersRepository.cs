using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models.Domain;

namespace TaskManagement.API.Repositories
{
    public class SQLUsersRepository : IUsersRepository
    {
        private readonly TaskManagementDbContext dbContext;
        public SQLUsersRepository(TaskManagementDbContext dbContext) 
        { 
            this.dbContext = dbContext;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }
    }
}
