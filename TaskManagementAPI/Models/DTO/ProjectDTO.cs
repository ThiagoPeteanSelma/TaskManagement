using System.Runtime.CompilerServices;
using TaskManagement.API.Models.Domain;

namespace TaskManagement.API.Models.DTO
{
    public class ProjectDTO
    {

        /// <summary>
        /// Identification Project
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Project Name
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Description Project
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// Maximum number of people allocated to the project
        /// </summary>
        public int TeamReach { get; set; }
        /// <summary>
        /// Total project resource
        /// </summary>
        public double Budget { get; set; }
        /// <summary>
        /// Date limit to finish project
        /// </summary>
        public required DateTime DeadLine { get; set; }
        /// <summary>
        /// Date to create the project
        /// </summary>
        public required DateTime DtCreateDate { get; set; }
        /// <summary>
        /// User who created the project. Considered the project manager
        /// </summary>
        public Guid UserId { get; set; }


        // Navigation properties
        public UserDTO? UserDTO { get; set; }

        public  IEnumerable<ProjectTaskDTO>? ProjectTaskDTO { get; set;}
#pragma warning disable CS8604 // Possible null reference argument.
        public int CountTask => ProjectTaskDTO.Count();
        public int CountTaskPending => ProjectTaskDTO.Where(x => x.Status == 1).Count();
        public int CountTaskInProgress => ProjectTaskDTO.Where(x => x.Status == 2).Count();
        public int CountTaskCompleted => ProjectTaskDTO.Where(x => x.Status == 3).Count();
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
