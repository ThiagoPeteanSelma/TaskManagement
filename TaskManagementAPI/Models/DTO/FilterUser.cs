using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Models.DTO
{
    public class FilterUser
    {
        public Guid? UserId { get; set; }
        public string? Email { get; set; }
    }
}
