using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models.Domain;

namespace TaskManagement.API.Data
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public required DbSet<User> Users { get; set; }
        public required DbSet<Project> Projects { get; set; }
        public required DbSet<ProjectTask> ProjectTasks { get; set; }
        public required DbSet<TaskComment> TaskComments { get; set; }
    }
}
