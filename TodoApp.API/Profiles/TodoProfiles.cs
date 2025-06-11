using AutoMapper;
using TodoApp.Data.DataModels.Dto.TodoDtos;
using TodoApp.Data.DataModels.Dto.UserDtos;
using TodoApp.Data.DataModels.Entities;

namespace TodoApp.API.Profiles
{
    public class TodoProfiles : Profile
    {
        public TodoProfiles()
        {
            CreateMap<TodoEntity, TodoReturnDto>();
            CreateMap<TodoReturnDto, TodoEntity>();

            CreateMap<TodoEntity, TodoAddDto>();
            CreateMap<TodoAddDto, TodoEntity>();

            CreateMap<TodoEntity, TodoToUpdateDto>();
            CreateMap<TodoToUpdateDto, TodoEntity>();

            CreateMap<UserEntity, RegisterDto>();
            CreateMap<RegisterDto, UserEntity>();

            CreateMap<UserEntity, LoginDto>();
            CreateMap<LoginDto, UserEntity>();
        }
    }
}
