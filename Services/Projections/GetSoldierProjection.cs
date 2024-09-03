namespace Services.Projections
{
    public record GetSoldierProjection(
        bool IncludeRank,
        bool IncludeCountry,
        bool IncludeSensorType);
}
