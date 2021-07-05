using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; } // By default, EF Core interprets a property that's named ID or classnameID as the primary key

        // attributes from DataAnnotations can control how classes and properties are mapped to the database.
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")] // caption for the text boxes
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")] // map the name of the FirstMidName property to "FirstName" in the database
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)] // The DataType attribute specifies a data type that's more specific than the database intrinsic type. In this case only the date should be displayed, not the date and time. DataType attributes DON'T provide validation.
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            // FullName is a calculated property with getter only (can't be set)
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public ICollection<Enrollment> Enrollments { get; set; } // collection navigation property: one-to-many. ICollection<T> can add or remove items
    }
}