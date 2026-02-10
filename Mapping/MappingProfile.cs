using AutoMapper;
using Vidly.Models;
using Vidly.Dtos;
using SQLitePCL;

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


       // Movie → MovieDto (for GET /api/movies and GetMovie)
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.GenreName,
                opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : null))

            // Critical: stop AutoMapper from trying to map the full nested Genre object
            .ForMember(dest => dest.Genre, opt => opt.Ignore());

        // MovieDto → Movie (for POST Create & PUT Update)
        CreateMap<MovieDto, Movie>()
            .ForMember(dest => dest.Id,        opt => opt.Ignore())       // client can't set ID
            .ForMember(dest => dest.DateAdded, opt => opt.Ignore())       // server sets this
            .ForMember(dest => dest.Genre,     opt => opt.Ignore());
    }
}
