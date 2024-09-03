using System;

namespace Services.DTOs
{
    public record GetAllResponseDTO(
        Guid SoldierId,
        string SoldierName,
        string CountryName,
        LocationDTO Location);
}
