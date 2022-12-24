using AutoMapper;
using HotDesk.Entities;
using HotDesk.Models;

namespace HotDesk
{
    public class HotDeskMappingProfile : Profile
    {
        public HotDeskMappingProfile() 
        {
            CreateMap<Location, LocationDto>();
            CreateMap<Desk, DeskDto>();

            CreateMap<AddLocationDto, Location>();
            CreateMap<AddDeskDto, Desk>();

            CreateMap<CreateUserDto, User>();

            CreateMap<DeskBookDto, Reservation>();
            CreateMap<Reservation, DeskBookDto>();
        }
    }
}
