using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public interface IProjectsRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(FilterUser filter);
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<Project> AddProjectAsync(Project project);
        Task<Project?> UpdateProjectAsync(Guid id, Project project);
        Task<Project?> DeleteProjectAsync(Guid id);
    }
}
