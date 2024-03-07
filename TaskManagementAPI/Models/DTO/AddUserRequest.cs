namespace TaskManagement.API.Models.DTO
{
    public class AddUserRequest
    {
        /// <summary>
        /// User name
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Email of user
        /// </summary>
        public required string Email { get; set; }
    }
}
