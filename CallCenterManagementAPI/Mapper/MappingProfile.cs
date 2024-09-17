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
			CreateMap<Call, UpdateCallDTO>().ReverseMap();
			CreateMap<Call, CreateCallDTO>().ReverseMap();
			CreateMap<Customer, UpdateCustomerDTO>().ReverseMap();
			CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
		}
	}
}
