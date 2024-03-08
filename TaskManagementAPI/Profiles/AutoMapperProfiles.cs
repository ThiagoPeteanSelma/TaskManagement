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
            CreateMap<FilterUser, User>().ReverseMap();
            CreateMap<AddUserRequest, User>().ReverseMap();

            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<FilterProject, Project>().ReverseMap();
            CreateMap<AddProjectRequest, Project>().ReverseMap();
            CreateMap<UpdateProjectRequest, Project>().ReverseMap();

        }
    }
}
