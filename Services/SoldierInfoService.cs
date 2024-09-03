using Services.DTOs;
using Services.Models;
using Services.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class SoldierInfoService : ISoldierInfoService
    {
        private readonly ISoldierInfoRepository _soldierInfoRepository;
        private readonly ISoldierLocationService _soldierLocationService;

        public SoldierInfoService(
            ISoldierInfoRepository soldierInfoRepository,
            ISoldierLocationService soldierLocationService)
        {
            _soldierInfoRepository = soldierInfoRepository
                ?? throw new ArgumentNullException(nameof(soldierInfoRepository));
            _soldierLocationService = soldierLocationService
                ?? throw new ArgumentNullException(nameof(soldierLocationService));
        }

        public async Task<IEnumerable<GetAllResponseDTO>> GetAllAsync(int pageSize, int pageIndex)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Must be greater than 0");
            }

            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Must be greater than 0");
            }

            var soldiers = await _soldierInfoRepository.GetAllAsync(
                pageSize,
                pageIndex,
                new GetSoldierProjection(
                    IncludeRank: false,
                    IncludeSensorType: false,
                    IncludeCountry: true));

            // improvement: create method to get more than one location at a time
            var result = new List<GetAllResponseDTO>(soldiers.Count());
            foreach (var soldier in soldiers)
            {
                var soldierLocation = await _soldierLocationService.GetSoldierCurrenLocationAsync(soldier.Id);
                result.Add(MapToGetAllDto(soldier, soldierLocation));
            }

            return result;
        }

        public async Task<GetSoldierInfoWithLocationResponseDTO> GetSoldierInfoWithLocationAsync(Guid soldierId)
        {
            var soldier = await _soldierInfoRepository.GetByIdAsync(
                soldierId,
                new Projections.GetSoldierProjection(
                    IncludeRank: true,
                    IncludeSensorType: true,
                    IncludeCountry: true));

            if (soldier == null)
            {
                return null;
            }

            var soldierLocation = await _soldierLocationService.GetSoldierCurrenLocationAsync(soldier.Id);

            return MapToGetSoldierInfoWithLocationDTO(soldier, soldierLocation);
        }

        private static GetSoldierInfoWithLocationResponseDTO MapToGetSoldierInfoWithLocationDTO(
            Soldier soldier, 
            LocationDTO soldierLocation)
        {
            return new(
                soldier.Id,
                soldier.Name,
                soldier.Rank?.Description ?? string.Empty,
                soldier.TrainingInfo,
                soldier.Country?.Name ?? string.Empty,
                soldier.SensorName,
                soldier.SensorType?.Description ?? string.Empty,
                soldierLocation);
        }

        private static GetAllResponseDTO MapToGetAllDto(
            Soldier soldier,
            LocationDTO soldierLocation)
        {
            return new(
                soldier.Id,
                soldier.Name,
                soldier.Country?.Name ?? string.Empty,
                soldierLocation);
        }
    }
}
