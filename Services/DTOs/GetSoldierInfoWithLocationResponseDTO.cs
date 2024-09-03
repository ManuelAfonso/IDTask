using System;

namespace Services.DTOs
{
    public record GetSoldierInfoWithLocationResponseDTO(
        Guid SoldierId,
        string SoldierName,
        string RankDescription,
        string TrainingInfo,
        string CountryName,
        string SensorName,
        string SensorTypeDescription,
        LocationDTO Location);

}
