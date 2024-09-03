using System;

namespace Repositories.Entities
{
    public class Soldier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RankId { get; set; }
        public int CountryId { get; set; }
        public string TrainingInfo { get; set; }
        public string SensorName { get; set; }
        public int SensorTypeId { get; set; }

        public Rank Rank { get; set; }
        public Country Country { get; set; }
        public SensorType SensorType { get; set; }
    }
}
