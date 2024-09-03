using Microsoft.EntityFrameworkCore;
using Services;
using Services.Models;
using Services.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models = Services.Models;

namespace Repositories
{
    public class SoldierInfoRepository : ISoldierInfoRepository
    {
        private readonly SoldierInfoContext _soldierInfoContext;
        public SoldierInfoRepository(SoldierInfoContext soldierInfoContext)
        {
            _soldierInfoContext = soldierInfoContext;
        }

        public async Task<IEnumerable<Models.Soldier>> GetAllAsync(
            int pageSize, 
            int pageIndex,
            GetSoldierProjection projection)
        {
            var skip = pageSize * (pageIndex - 1);

            var soldiers = _soldierInfoContext
                .Soldiers
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize);

            soldiers = ApplyProjection(projection, soldiers);

            var soldierList = await soldiers.ToListAsync();

            return soldierList.Select(MapToModel).ToList();
        }

        public async Task<Soldier> GetByIdAsync(
            Guid soldierId,
            GetSoldierProjection projection)
        {
            var soldiers = _soldierInfoContext
                .Soldiers
                .AsNoTracking()
                .Where(s => s.Id == soldierId);

            soldiers = ApplyProjection(projection, soldiers);

            var soldier = await soldiers.SingleOrDefaultAsync();

            return MapToModel(soldier);
        }

        private static IQueryable<Entities.Soldier> ApplyProjection(
            GetSoldierProjection projection, 
            IQueryable<Entities.Soldier> soldiers)
        {
            if (projection.IncludeCountry)
            {
                soldiers = soldiers.Include(s => s.Country);
            }

            if (projection.IncludeRank)
            {
                soldiers = soldiers.Include(s => s.Rank);
            }

            if (projection.IncludeSensorType)
            {
                soldiers = soldiers.Include(s => s.SensorType);
            }

            return soldiers;
        }

        private static Models.Soldier MapToModel(Entities.Soldier soldier)
        {
            if (soldier == null)
            {
                return null;
            }

            return new Models.Soldier
            {
                Id = soldier.Id,
                Name = soldier.Name,
                Country = new Models.Country
                {
                    Id = soldier.CountryId,
                    Name = soldier.Country?.Name ?? string.Empty
                },
                Rank = new Models.Rank
                {
                    Id = soldier.RankId,
                    Description = soldier.Rank?.Description ?? string.Empty
                },
                TrainingInfo = soldier.TrainingInfo,
                SensorName = soldier.SensorName,
                SensorType = new Models.SensorType
                {
                    Id = soldier.SensorTypeId,
                    Description = soldier.SensorType?.Description ?? string.Empty
                }
            };
        }
    }
}
