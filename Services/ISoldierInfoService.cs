using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ISoldierInfoService
    {
        Task<IEnumerable<GetAllResponseDTO>> GetAllAsync(int pageSize, int pageIndex);
        Task<GetSoldierInfoWithLocationResponseDTO> GetSoldierInfoWithLocationAsync(Guid soldierId);
    }
}