using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public interface IProjectTasksRepository
    {
        Task<IEnumerable<ProjectTask>> GetAllAsync(FilterProjectTask filter);
        Task<ProjectTask?> GetByIdAsync(Guid id);
        Task<ProjectTask> AddProjectTaskAsync(ProjectTask projectTask);
        Task<ProjectTask?> UpdateProjectTaskAsync(Guid id, ProjectTask projectTask);
        Task<ProjectTask?> DeleteProjectTaskAsync(Guid id);
    }
}
