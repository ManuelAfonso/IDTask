using Services.Models;
using System;

namespace Services.DTOs
{
    public record UpdateSoldierLocationRequestDTO(
        Guid SoldierId, 
        LocationDTO Location, 
        DateTime MovementDate, 
        SourceType SourceType);
}
