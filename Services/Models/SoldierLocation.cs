using System;

namespace Services.Models
{
    public class SoldierLocation
    {
        public long Id { get; set; }
        public Guid SoldierId { get; set; }
        public Location Location { get; set; }
        public DateTime MovementDate { get; set; }
        public SourceType SourceTypeId { get; set; }
        public bool Active { get; set; }
    }
}
