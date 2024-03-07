﻿using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;

namespace TaskManagement.API.Repositories
{
    public interface IProjectsRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(FilterProject filterProject);
        Task<Project?> GetByIdAsync(Guid projectId);
        Task<Project> AddProjectAsync(Project project);
    }
}