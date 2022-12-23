using HotDesk.Entities;
using HotDesk.Exceptions;
using HotDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDesk.Services
{
    public interface IReservationServices
    {
        int MakeOneDayReservation(DeskOneDayBookDto dto);
    }

    public class ReservationServices : IReservationServices
    {
        private IUserContextAccesorService _contextAccesor;
        private readonly HotDeskDbContext _dbContext;

        public ReservationServices(IUserContextAccesorService contextAccesor, HotDeskDbContext dbContext) 
        {
            _contextAccesor = contextAccesor;
            _dbContext = dbContext;
        }

        public int MakeOneDayReservation(DeskOneDayBookDto dto)
        {
            var location = _dbContext.Locations
                .Include(location => location.Desks)
                .FirstOrDefault(location => location.Id == dto.LocationId);

            if (location is null)
            {
                throw new NotFoundException("Location not found");
            }

            var desks = location.Desks.FirstOrDefault(desk => desk.Id == dto.DeskId);

            if(desks is null)
            {
                throw new NotFoundException("Desk not found");
            }

            var reservation = new Reservation()
            {
                LocationId = dto.LocationId,
                DeskId = dto.DeskId,
                UserId = _contextAccesor.UserId is null ? throw new ConflictException("Cannot make reservation without user id information") : (int)_contextAccesor.UserId,
                StartDate = dto.BookDay,
                EndDate = dto.BookDay,
            };

            _dbContext.Add(reservation);
            _dbContext.SaveChanges();

            return reservation.Id;
        }
    }
}
