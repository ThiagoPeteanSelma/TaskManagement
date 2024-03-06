using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Models.Domain
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        /// <summary>
        /// Identification User
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Email of user
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        /// Date to create the user
        /// </summary>
        public DateTime DtCreatedDate { get; set; }
        
    }
}
