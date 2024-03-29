﻿namespace TaskManagement.API.Models.DTO
{
    public class UserDTO
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
