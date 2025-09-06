namespace TaskManager_api.Helpers
{
    using AutoMapper;
    using TaskManager_api.DTOs.Auth;
    using TaskManager_api.DTOs.Board;
    using TaskManager_api.DTOs.BoardColumn;
    using TaskManager_api.DTOs.Project;
    using TaskManager_api.DTOs.ProjectUser;
    using TaskManager_api.DTOs.Task;
    using TaskManager_api.DTOs.User;
    using TaskManager_api.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ===== USER =====
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<User, UserProfileDTO>();

            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // custom hash ngoài service
                .ForMember(dest => dest.SystemRole, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ===== PROJECT =====
            CreateMap<ProjectCreateDTO, Project>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Project, ProjectResponseDTO>();

            // ===== BOARD =====
            CreateMap<BoardCreateDTO, Board>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Board, BoardResponseDTO>();

            // ===== BOARD COLUMN =====
            CreateMap<BoardColumnCreateDTO, BoardColumn>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<BoardColumn, BoardColumnResponseDTO>();

            // ===== TASK =====
            CreateMap<TaskItem, TaskDTO>()
                 .ForMember(dest => dest.CreatedAt,
                            opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                 .ForMember(dest => dest.UpdatedAt,
                            opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)))
                 .ForMember(dest => dest.AssignedToUserName,
                            opt => opt.MapFrom(src => src.AssignedToUser != null ? src.AssignedToUser.FullName : null))
                 .ForMember(dest => dest.CreatedByUserName,
                            opt => opt.MapFrom(src => src.CreatedByUser.FullName));

            CreateMap<TaskCreateDTO, TaskItem>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(_ => false));

            CreateMap<TaskUpdateDTO, TaskItem>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsArchived, opt => opt.Ignore());

            // ===== PROJECT USER =====
            CreateMap<ProjectUserAddDTO, ProjectUser>();
          
        }
    }


}
