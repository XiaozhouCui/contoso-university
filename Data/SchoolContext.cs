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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        // Fluent API vs Attributes (in entity classes): fluent API is used here only for database mapping that can't be done with attributes. However, the fluent API can specify most of the formatting, validation, and mapping rules that can be done with attributes.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // // use Fluent API to configure table names for each entity
            // modelBuilder.Entity<Course>().ToTable("Course");
            // modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            // modelBuilder.Entity<Student>().ToTable("Student");

            // use Fluent API to configure the many-to-many relationship between the Instructor and Course entities
            modelBuilder.Entity<Course>().ToTable(nameof(Course))
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);

            // use Fluent API to configure table names for each entity
            modelBuilder.Entity<Student>().ToTable(nameof(Student));
            modelBuilder.Entity<Instructor>().ToTable(nameof(Instructor));
            // Concurrency conflict handling: configures Department.ConcurrencyToken as a concurrency token.
            modelBuilder.Entity<Department>()
                .Property(d => d.ConcurrencyToken)
                .IsConcurrencyToken();
        }
    }
}
