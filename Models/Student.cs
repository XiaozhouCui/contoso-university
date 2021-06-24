using System;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; } // By default, EF Core interprets a property that's named ID or classnameID as the primary key
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } // collection navigation property: one-to-many. ICollection<T> can add or remove items
    }
}