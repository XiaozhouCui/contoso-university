using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

// this file was initialised by the following CLI command:
// dotnet aspnet-codegenerator razorpage -m Student -dc ContosoUniversity.Data.SchoolContext -udl -outDir Pages\Students --referenceScriptLibraries -sqlite

namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        // An entity set DbSet<TEntity> typically corresponds to a database table. LINQ queries against a DbSet<TEntity> will be translated into queries against the database.
        public DbSet<Student> Students { get; set; } // Student table will be available via "_context.Students"
        public DbSet<Enrollment> Enrollments { get; set; } // _context.Enrollments
        public DbSet<Course> Courses { get; set; } // _context.Courses
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure table names for each entity
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }
    }
}
