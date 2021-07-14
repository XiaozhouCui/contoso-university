namespace ContosoUniversity.Models.SchoolViewModels
{
    // this view model contains data to create the checkboxes for courses assigned to an instructor.
    public class AssignedCourseData
    {
        public int CourseID { get; set; } // show course ID, e.g. 4022
        public string Title { get; set; } // show course name, e.g. Microeconomics
        public bool Assigned { get; set; } // checkbox checked
    }
}