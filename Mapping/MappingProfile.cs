using AutoMapper;
using Vidly.Models;
using Vidly.Dtos;

namespace Vidly.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerDto, Customer>();

        CreateMap<MembershipType, MembershipTypeDto>();
        CreateMap<MembershipTypeDto, MembershipType>();

        // Add this override for update scenarios (ignores Id)
        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());


        CreateMap<Movie, MovieDto>();
        CreateMap<MovieDto, Movie>();
    }
}
