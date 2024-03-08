namespace TaskManagement.API.Models.DTO
{
    public class UpdateProjectTaskRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public StatusTask Status { get; set; }
        public required int Hours { get; set; }
    }
}
