using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // allows the app to specify the primary key rather than having the database generate it. Specified CourseID: a course number such as a 1000 series for the math department, a 2000 series for the English department
        [Display(Name = "Number")]
        public int CourseID { get; set; }
        
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        
        [Range(0, 5)]
        public int Credits { get; set; }
        public int DepartmentID { get; set; } // Freign Key, it is not necessary (EF can auto create FK) but it can make updates simpler and more efficient (with FK, don't have to fetch Department before update)
        public Department Department { get; set; } // reference navigation property: 1 course belongs to 1 department 
        public ICollection<Enrollment> Enrollments { get; set; } // collection navigation property: 1 Course has many students
        public ICollection<Instructor> Instructors { get; set; } // collection navigation property: 1 course may be taught by multiple instructors
    }
}