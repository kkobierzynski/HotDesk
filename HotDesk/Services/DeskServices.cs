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
        IEnumerable<DeskDto> AvailableUnavailableDesk(int locationId, AvailableDeskCheckDto dto);
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
        public IEnumerable<DeskDto> AvailableUnavailableDesk(int locationId, AvailableDeskCheckDto dto)
        {
            var location = _dbContext.Locations.FirstOrDefault(loc => loc.Id ==locationId);
            if (location == null)
            {
                throw new NotFoundException("Location not found");
            }
            var reservedIds = _dbContext.Reservations.Where(res => res.LocationId == locationId && 
                                                            res.StartDate <= dto.checkDate &&
                                                            res.EndDate >= dto.checkDate)
                                                     .Select(res => res.DeskId).Distinct();

            if (string.Equals(dto.availability, "available", StringComparison.CurrentCultureIgnoreCase))
            {
                var desks = _dbContext.Desks.Where(x => !reservedIds.Contains(x.Id) && x.LocationId == locationId);
                var desksDto = _mapper.Map<List<DeskDto>>(desks);
                return desksDto;
            }
            else if (string.Equals(dto.availability, "unavailable", StringComparison.CurrentCultureIgnoreCase))
            {
                var desks = _dbContext.Desks.Where(x => reservedIds.Contains(x.Id) && x.LocationId == locationId);
                var desksDto = _mapper.Map<List<DeskDto>>(desks);
                return desksDto;
            }
            else
            {
                throw new NotFoundException("Key value not found");
            }


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
