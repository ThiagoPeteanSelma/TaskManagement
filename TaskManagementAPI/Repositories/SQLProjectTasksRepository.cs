using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public class SQLProjectTasksRepository(TaskManagementDbContext dbContext) : IProjectTasksRepository
    {
        private readonly TaskManagementDbContext dbContext = dbContext;

        public async Task<ProjectTask> AddProjectTaskAsync(ProjectTask projectTask)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectTask?> DeleteProjectTaskAsync(Guid id)
        {
            var existingTask = await dbContext.ProjectTasks.FindAsync(id);
            if (existingTask == null)
            {
                return null;
            }

            dbContext.ProjectTasks.Remove(existingTask);

            await dbContext.SaveChangesAsync();

            return existingTask;
        }

        public async Task<IEnumerable<ProjectTask>> GetAllAsync(FilterProjectTask filter)
        {
            return await dbContext.ProjectTasks.Where(x => x.ProjectId == filter.ProjectId).Include(x => x.User).Include(x => x.Project).AsQueryable().ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await dbContext.ProjectTasks.Include(x => x.User).Include(x => x.Project).AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProjectTask?> UpdateProjectTaskAsync(Guid id, ProjectTask projectTask)
        {
            throw new NotImplementedException();
        }
    }
}
