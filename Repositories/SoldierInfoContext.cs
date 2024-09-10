using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class SoldierInfoContext : DbContext
    {
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<SensorType> SensorTypes { get; set; }
        public DbSet<Soldier> Soldiers { get; set; }

        public SoldierInfoContext(DbContextOptions<SoldierInfoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.HasDefaultSchema("SoldierInfo");

            // SoldierInfo schema configurations
            modelBuilder.Entity<Rank>()
                .ToTable("Rank", "SoldierInfo")
                .HasKey(r => r.Id);

            modelBuilder.Entity<Country>()
                .ToTable("Country", "SoldierInfo")
                .HasKey(c => c.Id);

            modelBuilder.Entity<SensorType>()
                .ToTable("SensorType", "SoldierInfo")
                .HasKey(st => st.Id);

            modelBuilder.Entity<Soldier>()
                .ToTable("Soldier", "SoldierInfo")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Soldier>()
                .HasOne(s => s.Rank)
                .WithMany()
                .HasForeignKey(s => s.RankId);

            modelBuilder.Entity<Soldier>()
                .HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey(s => s.CountryId);

            modelBuilder.Entity<Soldier>()
                .HasOne(s => s.SensorType)
                .WithMany()
                .HasForeignKey(s => s.SensorTypeId);
        }
    }
}
