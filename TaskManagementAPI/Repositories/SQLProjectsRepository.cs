using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public class SQLProjectsRepository(TaskManagementDbContext dbContext) : IProjectsRepository
    {
        private readonly TaskManagementDbContext dbContext = dbContext;

        public async Task<Project> AddProjectAsync(Project project)
        {
            project.Id = Guid.NewGuid();
            project.DtCreateDate = DateTime.Now;
            await dbContext.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> DeleteProjectAsync(Guid id)
        {
            var existingProject = await dbContext.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return null;
            }

            dbContext.Projects.Remove(existingProject);

            await dbContext.SaveChangesAsync();

            return existingProject;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(FilterUser filter)
        {
            Guid idUser = filter.UserId ?? Guid.Empty;

            if (idUser == Guid.Empty && !string.IsNullOrWhiteSpace(filter.Email)) {
                var user = await dbContext.Users.Where(x => x.Email == filter.Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    idUser = user.Id;
                }
            }

            return await dbContext.Projects.Where(x=> (idUser == Guid.Empty || x.UserId == idUser)).Include(x=> x.User).Include(x=> x.ProjectTasks).AsQueryable().ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await dbContext.Projects.Include(x => x.User).Include(x => x.ProjectTasks).AsQueryable().FirstOrDefaultAsync(x => x.Id == projectId);
        }

        public async Task<Project?> UpdateProjectAsync(Guid id, Project project)
        {
            var existProject = await dbContext.Projects.FindAsync(id);

            if (existProject == null)
            {
                return null;
            }

            existProject.Name = project.Name;
            existProject.Description = project.Description;
            existProject.TeamReach = project.TeamReach;
            existProject.DeadLine = project.DeadLine;
            existProject.Budget = project.Budget;

            await dbContext.SaveChangesAsync();

            return existProject;
        }
    }
}
