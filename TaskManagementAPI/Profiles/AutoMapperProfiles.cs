using AutoMapper;
using TaskManagement.API.Models.DTO;
using TaskManagement.API.Models.Domain;

namespace TaskManagement.API.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        { 
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Project, ProjectDTO>().ReverseMap();
        }
    }
}
