﻿namespace TaskManagement.API.Models.DTO
{
    public class UpdateProjectRequest
    {
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
    }
}
