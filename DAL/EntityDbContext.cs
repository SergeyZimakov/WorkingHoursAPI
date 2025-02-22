using Core.Entity.Auth;
using Core.Entity.DayType;
using Core.Entity.Shift;
using Core.Model.WorkRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class EntityDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public EntityDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Tables
        public DbSet<UserEntity> User { get; set; }
        public DbSet<DayTypeEntity> DayType { get; set; }
        public DbSet<AdditionalHoursEntity> AdditionalHours { get; set; }
        public DbSet<ShiftEntity> Shift { get; set; }
        public DbSet<ShiftPauseEntity> ShiftPause { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().ToTable("User", schema: "User");
            modelBuilder.Entity<DayTypeEntity>().ToTable("DayType", schema: "Settings");
            modelBuilder.Entity<AdditionalHoursEntity>().ToTable("AdditionalHours", schema: "Settings");
            modelBuilder.Entity<ShiftEntity>().ToTable("Shift", schema: "Shift");
            modelBuilder.Entity<ShiftPauseEntity>().ToTable("Pause", schema: "Shift");

            modelBuilder.Entity<UserEntity>().HasKey(e => e.UserID);
            modelBuilder.Entity<DayTypeEntity>().HasKey(e => e.DayTypeID);
            modelBuilder.Entity<AdditionalHoursEntity>().HasKey(e => new { e.DayTypeID, e.Order });
            modelBuilder.Entity<ShiftEntity>().HasKey(e => e.ShiftID);
            modelBuilder.Entity<ShiftPauseEntity>().HasKey(e => new { e.ShiftID });
            
            modelBuilder.Entity<AdditionalHoursEntity>()
                .HasOne(e => e.DayType)
                .WithMany(pe => pe.AdditionalHours)
                .HasForeignKey(e => e.DayTypeID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ShiftPauseEntity>()
                .HasOne(e => e.Shift)
                .WithMany(pe => pe.ShiftPauses)
                .HasForeignKey(e => e.ShiftID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
