using System;

namespace Services.DTOs
{
    public record GetLocationsInMapResponseDTO(
        Guid SoldierId,
        LocationDTO Location);
}
