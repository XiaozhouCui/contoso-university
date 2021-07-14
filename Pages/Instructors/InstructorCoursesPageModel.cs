using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.Pages.Instructors
{
    // this is the base class for the Edit and Create page models.
    public class InstructorCoursesPageModel : PageModel
    {
        // a list of view models (AssignedCourseData), each view model includes course ID, course title, and wether it's assigned (bool)
        public List<AssignedCourseData> AssignedCourseDataList; // this property is used to render the checkboxes

        // PopulateAssignedCourseData reads all Course entities to populate AssignedCourseDataList.
        public void PopulateAssignedCourseData(SchoolContext context, Instructor instructor)
        {
            var allCourses = context.Courses;
            // get a HashSet of course IDs for the given instructor (arg). A HashSet is used for efficient lookups.
            var instructorCourses = new HashSet<int>(
                instructor.Courses.Select(c => c.CourseID));
            // initialise an empty list of view model, to be populated by looping through the courses
            AssignedCourseDataList = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                // for each course, the code sets the CourseID, title, and whether or not the instructor is assigned to the course. 
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    // check if the instructor's course list contains the current course ID
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
        }
    }
}