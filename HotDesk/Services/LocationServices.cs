using AutoMapper;
using HotDesk.Entities;
using HotDesk.Exceptions;
using HotDesk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotDesk.Services
{
    public interface ILocationServices
    {
        IEnumerable<LocationDto> GetLocations();
        int CreateLocation(AddLocationDto dto);
        LocationDto ReturnLocationById(int id);
        void DeleteLocation(int id);
    }

    public class LocationServices : ILocationServices
    {
        private readonly HotDeskDbContext _dbContext;
        private readonly IMapper _mapper;

        public LocationServices(HotDeskDbContext dbContext, IMapper mapper) 
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        public IEnumerable<LocationDto> GetLocations()
        {
            var locations = _dbContext.Locations
                .Include(location => location.Desks)
                .ToList();

            var locationsDto = _mapper.Map<List<LocationDto>>(locations);

            return locationsDto;
        }

        public LocationDto ReturnLocationById(int id)
        {
            var location = _dbContext.Locations
                .Include(location => location.Desks)
                .FirstOrDefault(location => location.Id == id);

            if (location == null)
            {
                throw new NotFoundException("Location not found");
            }

            var locationDto = _mapper.Map<LocationDto>(location);

            return locationDto;
        }

        public int CreateLocation(AddLocationDto dto)
        {
            var location = _mapper.Map<Location>(dto);
            _dbContext.Locations.Add(location);
            _dbContext.SaveChanges();
            
            return location.Id;
        }

        public void DeleteLocation(int id)
        {
            var location = _dbContext.Locations
                .Include(location => location.Desks)
                .FirstOrDefault(location => location.Id == id);

            if (location == null)
            {
                throw new NotFoundException("Location not found");
            }
            if (!location.Desks.IsNullOrEmpty())
            {
                throw new ConflictException("Cannot delete location that contains desks");
            }

            _dbContext.Locations.Remove(location);
            _dbContext.SaveChanges();
        }
    }
}
