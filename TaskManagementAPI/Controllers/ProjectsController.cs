using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Diagnostics;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;
using TaskManagement.API.Repositories;

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
        /// 
        /// </summary>
        /// <param name="filterProject"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProjectsAsync([FromQuery] FilterProject filterProject)
        {
            if (!ValidateGetAllProjects(filterProject))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.


            var projectDTO = mapper.Map<IEnumerable<ProjectDTO>>(await projectRepository.GetAllAsync(filterProject));

            return !projectDTO.Any() ? NotFound() : Ok(projectDTO);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetProjectByIdAsync")]
        public async Task<IActionResult> GetProjectByIdAsync([FromRoute] Guid id)
        {
            var user = mapper.Map<ProjectDTO?>(await projectRepository.GetByIdAsync(id));

            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync([FromQuery] AddProjectRequest addProjectRequest)
        {
            if (!await ValidateCreateProjects(addProjectRequest))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var projectDomain = new Project()
            {
                Name = addProjectRequest.Name,
                Description = addProjectRequest.Description,
                Budget = addProjectRequest.Budget,
                DeadLine = addProjectRequest.DeadLine,
                TeamReach = addProjectRequest.TeamReach,
                UserId = addProjectRequest.IdUser ?? Guid.Empty
            };

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
        /// 
        /// </summary>
        /// <param name="filterProject"></param>
        /// <returns></returns>
        private bool ValidateGetAllProjects(FilterProject filterProject)
        {
            if (!filterProject.IdUser.HasValue && string.IsNullOrEmpty(filterProject.Email))
            {
                ModelState.AddModelError("ID or Email", "Please enter a email or user ID to search.");
            }

            return ModelState.ErrorCount <= 0;
        }

        private async Task<bool> ValidateCreateProjects(AddProjectRequest addProjectRequest)
        {
            if (!ValidateGetAllProjects(new FilterProject() { Email = addProjectRequest.Email, IdUser = addProjectRequest.IdUser }))
                return false;

            var user = await usersRepository.GetAllAsync(new FilterUser() { UserId = addProjectRequest.IdUser, Email = addProjectRequest.Email });
            if(!user.Any())
            {
                ModelState.AddModelError("User", "User not exist!");
            }

            return ModelState.ErrorCount <= 0;
        }
    }
}
