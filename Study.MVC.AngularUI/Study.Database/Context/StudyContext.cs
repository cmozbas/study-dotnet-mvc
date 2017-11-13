using Microsoft.EntityFrameworkCore;
using Study.Database.Entities;
using System;

namespace Study.Database
{
    public class StudyContext : DbContext
    {
        public DbSet<RadioStation> RadioStation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-5PDDBJC;Database=StudyMvcAngular;Trusted_Connection=True;");
        }
    }
}
