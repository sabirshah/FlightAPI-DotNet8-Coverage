using AutoMapper;
using FlightInformationAPI.DTOs;
using FlightInformationAPI.Enumerations;
using FlightInformationAPI.Models;

namespace FlightInformationAPI.Mapper
{

    public class FlightMappingProfile : Profile
    {
        public FlightMappingProfile()
        {
            CreateMap<Flight, FlightDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<FlightCreateDto, Flight>();
            CreateMap<FlightUpdateDto, Flight>();
        }
    }
}
