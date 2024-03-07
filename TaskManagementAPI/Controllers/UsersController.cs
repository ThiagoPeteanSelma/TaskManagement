using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Models.Domain;
using TaskManagement.API.Models.DTO;
using TaskManagement.API.Repositories;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMapper mapper, IUsersRepository userRepository) : ControllerBase
    {
        private readonly IMapper mapper = mapper;
        private readonly IUsersRepository userRepository = userRepository;
        /// <summary>
        /// Get all users in database or filter
        /// </summary>
        /// <param name="filterUser">if put a filter value, search the users with the filters in database else get all user in database</param>
        /// <returns>return a list with users exist in database</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] FilterUser filterUser)
        {
            var user = mapper.Map<IEnumerable<UserDTO>>(await userRepository.GetAllAsync(filterUser));

            return !user.Any() ? NotFound() : Ok(user);
        }
        /// <summary>
        /// Search user by identification
        /// </summary>
        /// <param name="id">identification user</param>
        /// <returns>return a object user</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetUserByIdAsync")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = mapper.Map<UserDTO?>(await userRepository.GetByIdAsync(id));

            return user == null? NotFound() : Ok(user);
        }
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="addUserRequest">Put the name and email for the new user</param>
        /// <returns>Return all information by user</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromQuery] AddUserRequest addUserRequest)
        {
            if (!await ValidateAddUserAsync(addUserRequest))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return BadRequest(string.Join(" ", ModelState.SelectMany(s => s.Value.Errors).Select(e => e.ErrorMessage)));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var userDomain = new User()
            {
                Name = addUserRequest.Name,
                Email = addUserRequest.Email
            };

            userDomain = await userRepository.AddUserAsync(userDomain);

            var userDTO = new UserDTO()
            {
                Id = userDomain.Id,
                Name = userDomain.Name,
                Email = userDomain.Email,
                DtCreatedDate = userDomain.DtCreatedDate
            };

            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = userDTO.Id }, userDTO);
        }
        /// <summary>
        /// Confirm if is possible add a new user.
        /// Check in database if the email exist for other user
        /// </summary>
        /// <param name="addUserRequest">received information about new user</param>
        /// <returns>return true if is valid and false if is not valid</returns>
        private async Task<bool> ValidateAddUserAsync(AddUserRequest addUserRequest)
        {
            var filter = new FilterUser() { Email = addUserRequest.Email };
            var users = await userRepository.GetAllAsync(filter);
            if (users.Any())
            {
                ModelState.AddModelError(nameof(addUserRequest.Email), $"The '{addUserRequest.Email.ToLower(System.Globalization.CultureInfo.CurrentCulture)}' is already registered for the user '{users.First().Name}'");
            }
            return ModelState.ErrorCount <= 0;
        }
    }
}
