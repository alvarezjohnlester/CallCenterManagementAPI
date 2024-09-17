using CallCenterManagementAPI.Model;
using AutoMapper;
using CallCenterManagementAPI.DTO;
namespace CallCenterManagementAPI.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Agent, UpdateAgentDTO>().ReverseMap();
			CreateMap<Agent, CreateAgentDTO>().ReverseMap();
			CreateMap<User, CreateUserDTO>().ReverseMap();
		}
	}
}
