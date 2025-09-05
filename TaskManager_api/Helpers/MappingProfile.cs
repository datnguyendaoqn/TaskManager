namespace TaskManager_api.Helpers
{
    using AutoMapper;
    using TaskManager_api.DTOs.Auth;
    using TaskManager_api.DTOs.User;
    using TaskManager_api.DTOs.Project;
    using TaskManager_api.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map User -> User(...)Dto
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<User, UserProfileDTO>();
                
            // Map RegisterDto -> User (trường hợp create user)
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // custom
                .ForMember(dest => dest.SystemRole, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Map Project->ProjectResponseDTO
                CreateMap<ProjectCreateDTO,Project>();
                CreateMap<Project, ProjectResponseDTO>();


        }
    }

}
