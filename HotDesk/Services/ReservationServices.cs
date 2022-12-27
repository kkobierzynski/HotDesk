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
        DeskBookDto GetReservationById(int id);
        void Update(int id, DeskBookDto dto);
        IEnumerable<ReservationDto> GetReservedByLocation(int locationId);
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

        public DeskBookDto GetReservationById(int id)
        {
            var reservation = _dbContext.Reservations.FirstOrDefault(res => res.Id == id);

            if (reservation == null)
            {
                throw new NotFoundException("Reservation not found");
            }

            var reservationDto = _mapper.Map<DeskBookDto>(reservation);
            return reservationDto;
        }

        public IEnumerable<ReservationDto> GetReservedByLocation(int locationId)
        {
            var roles = _dbContext.Roles;
            if (_dbContext.Locations.FirstOrDefault(x => x.Id == locationId) == null)
            {
                throw new NotFoundException("Location not found");
            }

            if (_contextAccesor.UserId == roles.FirstOrDefault(role => role.RoleName == "Administrator").Id)
            {
                var reservations = _dbContext.Reservations
                .Include(res => res.User)
                .Where(res => res.LocationId == locationId).ToList();

                var reservationsDto = _mapper.Map<List<ReservationDto>>(reservations);
                return reservationsDto;
            }
            else
            {
                var reservations = _dbContext.Reservations
                .Where(res => res.LocationId == locationId).ToList();

                var reservationsDto = _mapper.Map<List<ReservationDto>>(reservations);
                return reservationsDto;
            }

        }

        public void Update(int id, DeskBookDto dto)
        {
            var reservation = _dbContext.Reservations.FirstOrDefault(res => res.Id == id);
            
            if(reservation == null)
            {
                throw new NotFoundException("Desk reservation not found");
            }

            if (reservation.UserId != _contextAccesor.UserId)
            {
                throw new ForbiddenException("You are not allowed to change this reservation");
            }

            TimeSpan span = reservation.StartDate - DateTime.Now;
            if (span.TotalHours < 24)
            {
                throw new ConflictException("Cannot change reservation later than 24 hours before made reservation");
            }

            reservation.LocationId = dto.LocationId;
            reservation.DeskId = dto.DeskId;
            reservation.StartDate = dto.StartDate;
            reservation.EndDate = dto.EndDate;

            _dbContext.SaveChanges();
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
