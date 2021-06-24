using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; } // principal key
        public int CourseID { get; set; } // foreign key
        public int StudentID { get; set; } // foreign key
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; } // "?" means Grade is nullable

        public Course Course { get; set; } // reference navigation property: single Course entity
        public Student Student { get; set; } // reference navigation property: single Student entity
    }
}