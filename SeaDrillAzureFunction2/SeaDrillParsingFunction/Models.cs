using System;
using System.Collections.Generic;
using System.Text;

namespace SeaDrillParsingFunction.Models
{
    public class FileRecords
    {
        public string Country { get; set; } 
        public string Confirmed { get; set; }
        public string Deaths { get; set; }
        public string Recovered { get; set; }
        public string Active { get; set; }
        public string NewCases { get; set; }
        public string NewDeaths { get; set; }
        public string NewRecovered { get; set; }
        public string DeathsBy100Cases { get; set; }
        public string RecoveredBy100Cases { get; set; }
        public string DeathsBy100Recovered { get; set; }
        public string ConfirmedLastWeek { get; set; }
        public string OneWeekChange { get; set; }
        public string OneWeekPercentIncrease { get; set; }
        public string WHORegion { get; set; }

    }

    //public class AppDbContext : DbContext
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    //    public DbSet<FileRecords> FileRecords { get; set; }
    //}
}
