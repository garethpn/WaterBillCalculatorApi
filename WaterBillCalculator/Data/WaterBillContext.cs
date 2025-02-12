using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFramework;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace WaterBillCalculator.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class WaterBillContext : DbContext
    {
        public WaterBillContext(DbContextOptions<WaterBillContext> options)
            : base(options)
        {
        }

        public virtual Microsoft.EntityFrameworkCore.DbSet<BillDetails> Bills { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<MeterDetails> Meters { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<MeterReadings> Readings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillDetails>()
                .HasKey(b => b.Id);
            
            // Define the relationship between BillDetails and MeterReadings
            modelBuilder.Entity<MeterReadings>()
                .HasOne<BillDetails>()
                .WithMany(b => b.MeterReadings)
                .HasForeignKey(mr => mr.BillId);

            // Define the relationship between MeterDetails and MeterReadings
            modelBuilder.Entity<MeterReadings>()
                .HasOne<MeterDetails>(m => m.Meter)
                .WithMany(m => m.MeterReadings)
                .HasForeignKey(mr => mr.MeterId);
            
            // Define the relationship between MeterDetails and Parent/Child MeterDetails
            modelBuilder.Entity<MeterDetails>()
                .HasOne(m => m.ParentMeter)
                .WithMany(m => m.ChildMeters)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Seed data
            modelBuilder.Entity<MeterDetails>().HasData(
                new MeterDetails
                {
                    Id = 1, MeterName = "Peters Upper Field", MeterLocation = "Upper Field", MeterNumber = "0002"
                },
                new MeterDetails
                {
                    Id = 2, MeterName = "Peters Lower Field", MeterLocation = "Lower Field", MeterNumber = "0003",
                },
                new MeterDetails
                    { Id = 3, MeterName = "Our Field", MeterLocation = "Our Field", MeterNumber = "0004"},
                new MeterDetails
                    { Id = 4, MeterName = "The Houses", MeterLocation = "Just Before it Splits Between the Houses", MeterNumber = "0005"},
                new MeterDetails
                {
                    Id = 5, MeterName = "Riverbank Cottage", MeterLocation = "Riverbank Cottage", MeterNumber = "0006",
                    ParentId = 4
                },
                new MeterDetails
                {
                    Id = 6, MeterName = "Riverside Barn", MeterLocation = "Riverside Barn", MeterNumber = "0007",
                    ParentId = 4
                },
                new MeterDetails
                {
                    Id = 8, MeterName = "Waunwen Farm House", MeterLocation = "Waunwen Farm House", MeterNumber = "0008",
                    ParentId = 4
                });
        }
    }
}