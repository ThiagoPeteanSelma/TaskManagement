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

        public async Task<IEnumerable<Project>> GetAllAsync(FilterProject filterProject)
        {
            Guid idUser = filterProject.IdUser ?? Guid.Empty;

            if (idUser == Guid.Empty && !string.IsNullOrWhiteSpace(filterProject.Email)) {
                var user = await dbContext.Users.Where(x => x.Email == filterProject.Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    idUser = user.Id;
                }
            }

            return await dbContext.Projects.Where(x=> (idUser == Guid.Empty || x.UserId == idUser)).Include(x=> x.User).Include(x=> x.ProjectTasks).ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            return await dbContext.Projects.Include(x => x.User).Include(x => x.ProjectTasks).FirstOrDefaultAsync(x => x.Id == projectId);
        }
    }
}
