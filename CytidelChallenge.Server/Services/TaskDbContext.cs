using CytidelChallenge.Server.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;

namespace CytidelChallenge.Server.Services
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskRecord> Tasks { get; set; }

        private string _databasePath;

        public TaskDbContext(IConfiguration config)
        {
            _databasePath = config["ConnectionStrings:DatabaseFile"] ?? "";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TaskRecord>()
                .Property(e => e.Priority)
                .HasConversion<int>();

            modelBuilder
                .Entity<TaskRecord>()
                .Property(e => e.Status)
                .HasConversion<int>();
        }
    }
}
