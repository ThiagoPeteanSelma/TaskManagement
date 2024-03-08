namespace TaskManagement.API.Models.DTO
{
    public class AddProjectTaskRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public StatusTask Status { get; set; }
        public PriorityTask Priority { get; set; }
        public required int Hours { get; set; }
        public required Guid ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public string? Email { get; set; }
    }
}
