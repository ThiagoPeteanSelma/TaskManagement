namespace TaskManagement.API.Models.Domain
{
    public class TaskComment
    {
        /// <summary>
        /// Identification Comment of task
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Description comment
        /// </summary>
        public required string Comment { get; set; }
        /// <summary>
        /// date create the comment
        /// </summary>
        public DateTime DtCreateComment { get; set; }
        /// <summary>
        /// Identification the task
        /// </summary>
        public Guid ProjectTaskId { get; set; }
        /// <summary>
        /// identification user create the comment
        /// </summary>
        public Guid UserId { get; set; }

        // Navigation properties
        public required ProjectTask ProjectTask { get; set; }
        public required User User { get; set; }
    }
}
