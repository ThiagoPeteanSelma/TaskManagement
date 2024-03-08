using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Diagnostics;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;
using TaskManagement.API.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(IMapper mapper, IProjectsRepository projectRepository, IUsersRepository usersRepository) : ControllerBase
    {
        private readonly IMapper mapper = mapper;
        private readonly IProjectsRepository projectRepository = projectRepository;
        private readonly IUsersRepository usersRepository = usersRepository;
        /// <summary>
        /// List all projects by one user
        /// </summary>
        /// <param name="filter">find with iduser or email</param>
        /// <returns>list project by user</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProjectsAsync([FromQuery] FilterUser filter)
        {
            if (!ValidateGetAllProjects(filter))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.


            var projectDTO = mapper.Map<IEnumerable<ProjectDTO>>(await projectRepository.GetAllAsync(filter));

            return !projectDTO.Any() ? NotFound() : Ok(projectDTO);
        }
        /// <summary>
        /// find project with idproject
        /// </summary>
        /// <param name="id">identification project</param>
        /// <returns>return a project</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetProjectByIdAsync")]
        public async Task<IActionResult> GetProjectByIdAsync([FromRoute] Guid id)
        {
            var user = mapper.Map<ProjectDTO?>(await projectRepository.GetByIdAsync(id));

            return user == null ? NotFound() : Ok(user);
        }
        /// <summary>
        /// create a new project
        /// </summary>
        /// <param name="addProjectRequest">all fields with information about the project</param>
        /// <returns>return a project created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync([FromQuery] AddProjectRequest addProjectRequest)
        {
            if (!await ValidateCreateProjects(addProjectRequest))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8604 // Possible null reference argument.
            var projectDomain = new Project()
            {
                Name = addProjectRequest.Name,
                Description = addProjectRequest.Description,
                Budget = addProjectRequest.Budget,
                DeadLine = addProjectRequest.DeadLine,
                TeamReach = addProjectRequest.TeamReach,
                UserId = addProjectRequest.UserId ?? await usersRepository.GetIdByEmail(addProjectRequest.Email),
            };
#pragma warning restore CS8604 // Possible null reference argument.

            projectDomain = await projectRepository.AddProjectAsync(projectDomain);

            var projectDTO = new ProjectDTO()
            {
                Name = projectDomain.Name,
                Description = projectDomain.Description,
                Budget = projectDomain.Budget,
                DeadLine = projectDomain.DeadLine,
                TeamReach = projectDomain.TeamReach,
                DtCreateDate = projectDomain.DtCreateDate,
                UserId = projectDomain.UserId
            };

            return CreatedAtAction(nameof(GetProjectByIdAsync), new { id = projectDTO.Id }, projectDTO);
        }
        /// <summary>
        /// change values project
        /// </summary>
        /// <param name="id">identification project</param>
        /// <param name="updateProjectRequest">fields values change project</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateProjectAsync([FromRoute] Guid id, [FromQuery] UpdateProjectRequest updateProjectRequest)
        {
            if (!await ValidateUpdateProject(id))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var projectDomainModel = mapper.Map<Project>(updateProjectRequest);

            projectDomainModel = await projectRepository.UpdateProjectAsync(id, projectDomainModel);

            return projectDomainModel == null ? NotFound() : Ok(mapper.Map<ProjectDTO>(projectDomainModel));
        }
        /// <summary>
        /// Delete a project if all tasks are completed or not exist tasks
        /// </summary>
        /// <param name="id">identification project</param>
        /// <returns>return a project delete</returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProjectAsync([FromRoute] Guid id)
        {
            if (!await ValidateDeleteProject(id))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var projectDelete = projectRepository.DeleteProjectAsync(id);

            return projectDelete == null ? NotFound() : Ok(mapper.Map<ProjectDTO>(projectDelete));
        }
        /// <summary>
        /// check if put a email or iduser
        /// </summary>
        /// <param name="filterProject">field id user and email</param>
        /// <returns>return true if user put id or email and false if user not put any data</returns>
        private bool ValidateGetAllProjects(FilterUser filter)
        {
            if (!filter.UserId.HasValue && string.IsNullOrEmpty(filter.Email))
            {
                ModelState.AddModelError("ID or Email", "Please enter a email or user ID to search.");
            }

            return ModelState.ErrorCount <= 0;
        }
        /// <summary>
        /// check if user put id user or email
        /// Check if the user exist in database
        /// </summary>
        /// <param name="addProjectRequest">receive data to create the project</param>
        /// <returns>if user don´t exist return false and true if exist</returns>
        private async Task<bool> ValidateCreateProjects(AddProjectRequest addProjectRequest)
        {
            if (!ValidateGetAllProjects(new FilterUser() { Email = addProjectRequest.Email, UserId = addProjectRequest.UserId }))
                return false;

            var user = await usersRepository.GetAllAsync(new FilterUser() { UserId = addProjectRequest.UserId, Email = addProjectRequest.Email });
            if (!user.Any())
                ModelState.AddModelError("User", "User not exist!");

            return ModelState.ErrorCount <= 0;
        }
        /// <summary>
        /// check if project exist
        /// </summary>
        /// <param name="id">identification project</param>
        /// <returns>return true if project exist</returns>
        private async Task<bool> ValidateUpdateProject(Guid id)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project == null)
                ModelState.AddModelError("Project", "Project not exist!");

            return ModelState.ErrorCount <= 0;
        }
        /// <summary>
        /// Check if exist project
        /// Check all task are completed
        /// </summary>
        /// <param name="id">identification id</param>
        /// <returns>return true if exist project and all tasks are completed</returns>
        private async Task<bool> ValidateDeleteProject(Guid id)
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project == null)
                ModelState.AddModelError("Project", "Project not exist!");
            ///Business Rules
            ///2. **Restrições de Remoção de Projetos:**
            ///-Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
            ///Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (project.ProjectTasks != null)
                if (project.ProjectTasks.Count() != project.ProjectTasks.Count(x => x.Status == StatusTask.Completed))
                    ModelState.AddModelError("Task", "One or more task need completed or delete!");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return ModelState.ErrorCount <= 0;
        }
    }
}