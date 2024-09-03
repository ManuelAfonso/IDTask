using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ISoldierLocationService
    {
        public Task UpdateSoldierLocationAsync(UpdateSoldierLocationRequestDTO soldierLocationDTO);
        Task<LocationDTO> GetSoldierCurrenLocationAsync(Guid soldierId);
        Task<IEnumerable<GetLocationsInMapResponseDTO>> GetLocationsInMapAsync(LocationDTO upperLeftLocation, LocationDTO bottomRightLocation);
    }
}
