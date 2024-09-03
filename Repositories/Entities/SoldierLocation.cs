using System;

namespace Repositories.Entities
{
    public class SoldierLocation
    {
        public long Id { get; set; }
        public Guid SoldierId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime MovementDate { get; set; }
        public int SourceTypeId { get; set; }
        public bool Active { get; set; }

        public SourceType SourceType { get; set; }
    }
}
