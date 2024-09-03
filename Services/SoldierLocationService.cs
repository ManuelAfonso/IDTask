using Services.DTOs;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class SoldierLocationService : ISoldierLocationService
    {
        private readonly ISoldierLocationRepository _soldierLocationRepository;
        private readonly ITimeProvider _timeProvider;

        public SoldierLocationService(
            ISoldierLocationRepository soldierLocationRepository,
            ITimeProvider timeProvider)
        {
            _soldierLocationRepository = soldierLocationRepository
                ?? throw new ArgumentNullException(nameof(soldierLocationRepository));
            _timeProvider = timeProvider 
                ?? throw new ArgumentNullException(nameof(_timeProvider));
        }

        public async Task<IEnumerable<GetLocationsInMapResponseDTO>> GetLocationsInMapAsync(LocationDTO upperLeftLocation, LocationDTO bottomRightLocation)
        {
            if (upperLeftLocation == null)
            {
                throw new ArgumentNullException(nameof(upperLeftLocation));
            }
            
            if (bottomRightLocation == null)
            {
                throw new ArgumentNullException(nameof(bottomRightLocation));
            }
            
            if (bottomRightLocation.Latitude <= upperLeftLocation.Latitude || 
                bottomRightLocation.Longitude <= upperLeftLocation.Longitude)
            {
                throw new ArgumentOutOfRangeException(nameof(bottomRightLocation), "BottomRight values must be greater than UpperLeft values");
            }

            var locations =
                await _soldierLocationRepository.GetLocationsInMapAsync(
                    MapLocationToModel(upperLeftLocation),
                    MapLocationToModel(bottomRightLocation));

            return locations.Select(p =>
                new GetLocationsInMapResponseDTO(
                    p.SoldierId,
                    MapLocationToDTO(p.Location)));
        }

        public async Task<LocationDTO> GetSoldierCurrenLocationAsync(Guid soldierId)
        {
            var soldierLocation = await _soldierLocationRepository.GetSoldierCurrenLocationAsync(soldierId);
            if (soldierLocation == null)
            {
                return null;
            }

            return MapLocationToDTO(soldierLocation.Location);
        }

        public async Task UpdateSoldierLocationAsync(UpdateSoldierLocationRequestDTO soldierLocationDTO)
        {
            if (soldierLocationDTO == null)
            {
                throw new ArgumentNullException(nameof(soldierLocationDTO));
            }

            if (soldierLocationDTO.SoldierId == Guid.Empty)
            {
                throw new ArgumentException("SoldierId cannot be an empty Guid", nameof(soldierLocationDTO));
            }

            if (soldierLocationDTO.Location == null)
            {
                throw new ArgumentException("Location cannot be null", nameof(soldierLocationDTO));
            }

            if (Math.Abs(soldierLocationDTO.Location.Latitude) > 90)
            {
                throw new ArgumentException("Invalid valur for Latitude", nameof(soldierLocationDTO));
            }

            if (Math.Abs(soldierLocationDTO.Location.Longitude) > 180)
            {
                throw new ArgumentException("Invalid valur for Longitude", nameof(soldierLocationDTO));
            }

            if (soldierLocationDTO.MovementDate > _timeProvider.GetUTCNow().AddMilliseconds(5))
            {
                throw new ArgumentException("MovementDate must be lesser than current time", nameof(soldierLocationDTO));
            }

            var newLocation = new SoldierLocation
            {
                SoldierId = soldierLocationDTO.SoldierId,
                Location = MapLocationToModel(soldierLocationDTO.Location),
                MovementDate = soldierLocationDTO.MovementDate,
                SourceTypeId = soldierLocationDTO.SourceType,
                Active = true
            };

            await _soldierLocationRepository.UpdateSoldierLocationAsync(newLocation);
        }

        private static Models.Location MapLocationToModel(LocationDTO location)
        {
            if (location == null) 
            { 
                return null; 
            }

            return new Location
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        private static LocationDTO MapLocationToDTO(Models.Location location)
        {
            if (location == null)
            {
                return null;
            }

            return new LocationDTO(location.Latitude, location.Longitude);
        }
    }
}
