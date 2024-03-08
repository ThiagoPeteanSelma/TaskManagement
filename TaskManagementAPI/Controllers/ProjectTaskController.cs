using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;
using TaskManagement.API.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTaskController(IMapper mapper, IProjectTasksRepository projectTaskRepository, IProjectsRepository projectRepository, IUsersRepository usersRepository) : ControllerBase
    {
        private readonly IMapper mapper = mapper;
        private readonly IProjectTasksRepository projectTaskRepository = projectTaskRepository;
        private readonly IProjectsRepository projectRepository = projectRepository;
        private readonly IUsersRepository usersRepository = usersRepository;
        [HttpGet]
        public async Task<IActionResult> GetAllProjectTask([FromQuery] FilterProjectTask filter)
        {
            var projectDTO = mapper.Map<IEnumerable<ProjectDTO>>(await projectTaskRepository.GetAllAsync(filter));

            return !projectDTO.Any() ? NotFound() : Ok(projectDTO);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetProjectTaskByIdAsync")]
        public async Task<IActionResult> GetProjectTaskByIdAsync([FromRoute] Guid id)
        {
            var user = mapper.Map<ProjectDTO?>(await projectTaskRepository.GetByIdAsync(id));

            return user == null ? NotFound() : Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProjectTaskAsync([FromQuery] AddProjectTaskRequest addProjectTaskRequest)
        {
            if (!await ValidateCreateProjectTask(addProjectTaskRequest))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var projectTaskDomain = new ProjectTask()
            {
                Name = addProjectTaskRequest.Name,
                Description = addProjectTaskRequest.Description,
                Status = addProjectTaskRequest.Status,
                Priority = addProjectTaskRequest.Priority,
                Hours = addProjectTaskRequest.Hours,
                ProjectId = addProjectTaskRequest.ProjectId,
                UserId = addProjectTaskRequest.UserId ?? await usersRepository.GetIdByEmail(addProjectTaskRequest.Email ?? string.Empty)
            };

            projectTaskDomain = await projectTaskRepository.AddProjectTaskAsync(projectTaskDomain);

            var projectTaskDTO = new ProjectTaskDTO()
            {
                Id = projectTaskDomain.Id,
                Name = projectTaskDomain.Name,
                Description = projectTaskDomain.Description,
                Priority = projectTaskDomain.Priority,
                Hours = projectTaskDomain.Hours,
                DtCreateTask = projectTaskDomain.DtCreateTask,
                Status = projectTaskDomain.Status,
                DtCompletedTask = projectTaskDomain.DtCompletedTask,
                ProjectId = projectTaskDomain.ProjectId,
                UserId = projectTaskDomain.UserId
            };

            return CreatedAtAction(nameof(GetProjectTaskByIdAsync), new { id = projectTaskDTO.Id }, projectTaskDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateProjectTaskAsync([FromRoute] Guid id, [FromQuery] UpdateProjectTaskRequest updateProjectRequest)
        {
            var projectTaskDomainModel = mapper.Map<ProjectTask>(updateProjectRequest);

            projectTaskDomainModel = await projectTaskRepository.UpdateProjectTaskAsync(id, projectTaskDomainModel);

            return projectTaskDomainModel == null ? NotFound() : Ok(mapper.Map<ProjectTaskDTO>(projectTaskDomainModel));
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProjectAsync([FromRoute] Guid id)
        {
            var projectTaskDelete = projectTaskRepository.DeleteProjectTaskAsync(id);

            return projectTaskDelete == null ? NotFound() : Ok(mapper.Map<ProjectTaskDTO>(projectTaskDelete));
        }

        private async Task<bool> ValidateCreateProjectTask(AddProjectTaskRequest addProjectTaskRequest)
        {
            var project = await projectRepository.GetByIdAsync(addProjectTaskRequest.ProjectId);
            if (project == null)
                ModelState.AddModelError("Project", "Project not exist!");

            if (!addProjectTaskRequest.UserId.HasValue && string.IsNullOrEmpty(addProjectTaskRequest.Email))
                ModelState.AddModelError("ID or Email", "Please enter a email or user ID to search.");
            else
            {
                var user = await usersRepository.GetAllAsync(new FilterUser() { UserId = addProjectTaskRequest.UserId, Email = addProjectTaskRequest.Email });
                if (!user.Any())
                    ModelState.AddModelError("User", "User not exist!");
            }
            /// Business Rules: 
            /// 4. **Limite de Tarefas por Projeto:**
            ///   -Cada projeto tem um limite máximo de 20 tarefas.Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
            var tasks = await projectTaskRepository.GetAllAsync(new FilterProjectTask() { ProjectId = addProjectTaskRequest.ProjectId });
            if (tasks.Any())
                if (tasks.Where(x => x.Status != StatusTask.Completed).Count() == 20)
                    ModelState.AddModelError("Task", "Exceeded the allowed number of open tasks!");

            return ModelState.ErrorCount <= 0;
        }
    }
}
