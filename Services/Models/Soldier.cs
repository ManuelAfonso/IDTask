using System;

namespace Services.Models
{
    public class Soldier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TrainingInfo { get; set; }
        public string SensorName { get; set; }
        public Rank Rank { get; set; }
        public Country Country { get; set; }
        public SensorType SensorType { get; set; }
    }
}
