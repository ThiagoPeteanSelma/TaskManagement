using TaskManagement.API.Models.Domain;

namespace TaskManagement.API.Models.DTO
{
    public class ProjectTaskDTO
    {
        /// <summary>
        /// Identification Task
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// task Name
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Description task
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// Priority that the task was defined, only accepts values (1 = "Low", 2 = "Medium" and 3 = "High")
        /// </summary>
        public PriorityTask Priority { get; set; }
        /// <summary>
        /// Hours to completed task
        /// </summary>
        public required int Hours { get; set; }
        /// <summary>
        /// Date to create the task
        /// </summary>
        public required DateTime DtCreateTask { get; set; }
        /// <summary>
        /// Identifies status of the task, only accepts values (1 = "Pending", 2 = "In Progress" and 3 = "Completed")
        /// </summary>
        public StatusTask Status { get; set; }
        /// <summary>
        /// Date to completed the task
        /// </summary>
        public DateTime? DtCompletedTask { get; set; }
        /// <summary>
        /// Project that belongs to task
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// User who must execute the task
        /// </summary>
        public Guid UserId { get; set; }

        // Navigation properties
        public ProjectDTO? Project { get; set; }
        public UserDTO? User { get; set; }
    }
}
