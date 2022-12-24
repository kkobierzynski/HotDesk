using AutoMapper;
using HotDesk.Entities;
using HotDesk.Exceptions;
using HotDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDesk.Services
{
    public interface IReservationServices
    {
        int MakeReservation(DeskBookDto dto);
    }

    public class ReservationServices : IReservationServices
    {
        private readonly IUserContextAccesorService _contextAccesor;
        private readonly HotDeskDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReservationServices(IUserContextAccesorService contextAccesor, HotDeskDbContext dbContext, IMapper mapper) 
        {
            _contextAccesor = contextAccesor;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int MakeReservation(DeskBookDto dto)
        {
            var location = _dbContext.Locations
            .Include(location => location.Desks)
            .FirstOrDefault(location => location.Id == dto.LocationId);

            if (location is null)
            {
                throw new NotFoundException("Location not found");
            }

            var desks = location.Desks.FirstOrDefault(desk => desk.Id == dto.DeskId);

            if (desks is null)
            {
                throw new NotFoundException("Desk not found");
            }

            var reservation = _mapper.Map<Reservation>(dto);

            reservation.UserId = _contextAccesor.UserId is null ? throw new ConflictException("Cannot make reservation without user id information") : (int)_contextAccesor.UserId;

            _dbContext.Add(reservation);
            _dbContext.SaveChanges();

            return reservation.Id;
        }


    }
}
