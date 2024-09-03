using Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ISoldierLocationRepository
    {
        Task<IEnumerable<SoldierLocation>> GetLocationsInMapAsync(Location upperLeftLocation, Location bottomRightLocation);
        Task<SoldierLocation> GetSoldierCurrenLocationAsync(Guid soldierId);
        Task UpdateSoldierLocationAsync(SoldierLocation newLocation);
    }
}
