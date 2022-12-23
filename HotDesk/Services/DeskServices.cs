using AutoMapper;
using HotDesk.Entities;
using HotDesk.Exceptions;
using HotDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDesk.Services
{
    public interface IDeskServices
    {
        DeskDto GetDeskById(int locationId, int deskId);
        int CreateDesk(int locationId, AddDeskDto dto);
        void DeleteDesk(int locationId, int deskId);
    }
    public class DeskServices : IDeskServices
    {
        private readonly HotDeskDbContext _dbContext;
        private readonly IMapper _mapper;
        public DeskServices(HotDeskDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public DeskDto GetDeskById(int locationId, int deskId)
        {
            var location = _dbContext.Locations
                .Include(location => location.Desks)
                .FirstOrDefault(location => location.Id == locationId);

            if (location == null)
            {
                throw new NotFoundException("Location not found");
            }

            var desk = location.Desks.FirstOrDefault(desk => desk.Id == deskId);

            if (desk == null)
            {
                throw new NotFoundException("Desk not found");
            }

            var deskdto = _mapper.Map<DeskDto>(desk);
            return deskdto;

        }

        public int CreateDesk(int locationId, AddDeskDto dto)
        {
            var location = _dbContext.Locations.FirstOrDefault(location => location.Id == locationId);
            if (location == null)
            {
                throw new NotFoundException("The location you want to add desks to cannot be found");
            }

            var deskDto = _mapper.Map<Desk>(dto);
            deskDto.LocationId = location.Id;

            _dbContext.Desks.Add(deskDto);
            _dbContext.SaveChanges();

            return deskDto.Id;
        }

        public void DeleteDesk(int locationId, int deskId)
        {
            var location = _dbContext.Locations
                .Include(location => location.Desks)
                .FirstOrDefault(location => location.Id == locationId);

            if (location == null)
            {
                throw new NotFoundException("The location you want to remove desks from cannot be found");
            }

            var desk = location.Desks.FirstOrDefault(desk => desk.Id == deskId);

            if (desk == null)
            {
                throw new NotFoundException("Desk not found");
            }

            var isReserved = _dbContext.Reservations.Any(reservation => reservation.DeskId == desk.Id); //Check in Postman
            if (isReserved)
            {
                throw new ConflictException("Cannot delele desk that is reserved");
            }

            _dbContext.Desks.Remove(desk);
            _dbContext.SaveChanges();
        }
    }
}
