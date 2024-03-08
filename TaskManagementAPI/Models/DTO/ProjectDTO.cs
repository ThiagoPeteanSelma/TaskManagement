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
        public UserDTO? User { get; set; }

        public  IEnumerable<ProjectTaskDTO>? ProjectTasks { get; set;}
        public int CountTask => ProjectTasks == null ? 0 : ProjectTasks.Count();
        public int CountTaskPending => ProjectTasks == null ? 0 : ProjectTasks.Where(x => x.Status == (int)StatusTask.Pending).Count();
        public int CountTaskInProgress => ProjectTasks == null ? 0 : ProjectTasks.Where(x => x.Status == (int)StatusTask.In_Progress).Count();
        public int CountTaskCompleted => ProjectTasks == null ? 0 : ProjectTasks.Where(x => x.Status == (int)StatusTask.Completed).Count();
    }
}
