using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")] // Column attribute is used to change SQL data type mapping. The Budget column is defined using the SQL Server money type in the database
        public decimal Budget { get; set; } // Budget is for currency, and the money data type is more appropriate for currency.

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                       ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /* if the "Department.InstructorID" property was defined as non-nullable, EF Core would configure a cascade delete rule. The department would be deleted when the instructor assigned as its administrator is deleted. To avoid cascade delete, add restrict rule in Fluent API (modelBuilder) */
        public int? InstructorID { get; set; } // FK. A department may or may not have an administrator, int? means nullable

        public Instructor Administrator { get; set; } // An administrator is always an instructor (1-to-1), so InstructorID is included as FK
        public ICollection<Course> Courses { get; set; } // A department may have many courses, so there's a Courses navigation property
    }
}