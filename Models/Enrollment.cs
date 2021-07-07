using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    // An enrollment record is for one course taken by one student. It functions as a many-to-many join table with payload (Grade) in the database
    public class Enrollment
    {
        public int EnrollmentID { get; set; } // principal key
        public int CourseID { get; set; } // Foreign Key, an enrollment record is for one course (1-to-1)
        public int StudentID { get; set; } // Foreign Key, an enrollment record is for one student (1-to-1)
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; } // "?" means Grade is nullable

        public Course Course { get; set; } // reference navigation property: single Course entity
        public Student Student { get; set; } // reference navigation property: single Student entity
    }
}