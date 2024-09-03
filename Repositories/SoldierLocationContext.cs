using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class SoldierLocationContext : DbContext
    {
        public DbSet<SourceType> SourceTypes { get; set; }
        public DbSet<Entities.SoldierLocation> SoldierLocations { get; set; }

        public SoldierLocationContext(DbContextOptions<SoldierLocationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SourceType>()
                .ToTable("SourceType", "SoldierLocation")
                .HasKey(st => st.Id);

            modelBuilder.Entity<Entities.SoldierLocation>()
                .ToTable("SoldierLocation", "SoldierLocation")
                .HasKey(sl => sl.Id);

            modelBuilder.Entity<Entities.SoldierLocation>()
                .HasOne(sl => sl.SourceType)
                .WithMany()
                .HasForeignKey(sl => sl.SourceTypeId);
        }
    }
}
