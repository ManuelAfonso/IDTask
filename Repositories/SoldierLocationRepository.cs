using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Services.Models;

namespace Repositories
{
    public class SoldierLocationRepository : ISoldierLocationRepository
    {
        private readonly SoldierLocationContext _soldierLocationContext;
        public SoldierLocationRepository(SoldierLocationContext soldierLocationContext)
        {
            _soldierLocationContext = soldierLocationContext;
        }

        public async Task<Models.SoldierLocation> GetSoldierCurrenLocationAsync(Guid soldierId)
        {
            var location = await _soldierLocationContext
                .SoldierLocations
                .AsNoTracking()
                .Where(s => s.Active && s.SoldierId == soldierId)
                .FirstOrDefaultAsync();

            return MapToModel(location);
        }

        public async Task UpdateSoldierLocationAsync(Models.SoldierLocation newLocation)
        {
            var current = await _soldierLocationContext
                .SoldierLocations
                .Where(s => s.Active && s.SoldierId == newLocation.SoldierId)
                .ToListAsync();

            foreach ( var item in current )
            {
                item.Active = false;
            }

            var ent = MapToEntity(newLocation);

            _soldierLocationContext.SoldierLocations.Add(ent);

            await _soldierLocationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.SoldierLocation>> GetLocationsInMapAsync(
            Models.Location upperLeftLocation, 
            Models.Location bottomRightLocation)
        {
            var locations = await _soldierLocationContext
                .SoldierLocations
                .AsNoTracking()
                .Where(s => 
                    s.Active && 
                    s.Latitude >= upperLeftLocation.Latitude &&
                    s.Latitude <= bottomRightLocation.Latitude &&
                    s.Longitude >= upperLeftLocation.Longitude &&
                    s.Longitude <= bottomRightLocation.Longitude)
                .ToListAsync();

            return locations.Select(MapToModel);
        }

        private static Models.SoldierLocation MapToModel(Entities.SoldierLocation soldierLocation)
        {
            if (soldierLocation == null)
            {
                return null;
            }

            return new Models.SoldierLocation
            {
                Id = soldierLocation.Id,
                SoldierId = soldierLocation.SoldierId,
                Active = soldierLocation.Active,
                Location = new Models.Location
                {
                    Latitude = soldierLocation.Latitude,
                    Longitude = soldierLocation.Longitude,
                },
                MovementDate = soldierLocation.MovementDate,
                SourceTypeId = (Services.Models.SourceType)soldierLocation.SourceTypeId,
            };
        }

        private static Entities.SoldierLocation MapToEntity(Models.SoldierLocation soldierLocation)
        {
            return new Entities.SoldierLocation
            {
                SoldierId = soldierLocation.SoldierId,
                Active = soldierLocation.Active,
                MovementDate = soldierLocation.MovementDate,
                Latitude = soldierLocation.Location.Latitude,
                Longitude = soldierLocation.Location.Longitude,
                SourceTypeId = (int)soldierLocation.SourceTypeId
            };
        }
    }
}
