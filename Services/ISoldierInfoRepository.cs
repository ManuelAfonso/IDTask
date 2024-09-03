using Services.Models;
using Services.Projections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ISoldierInfoRepository
    {
        Task<IEnumerable<Soldier>> GetAllAsync(int pageSize, int pageIndex, GetSoldierProjection projection);
        Task<Soldier> GetByIdAsync(Guid soldierId, GetSoldierProjection projection);
    }
}
