using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;  // Add VM
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; } // to store the ID of the SELECTED instructor
        public int CourseID { get; set; }

        // The OnGetAsync method accepts optional route data for the ID of the selected instructor.
        public async Task OnGetAsync(int? id, int? courseID)
        {
            // initialize a new view model "InstructorData", which contains all 3 entities
            InstructorData = new InstructorIndexData();
            // load all instructor-related data from DbContext into InstructorData.Instructors
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment) // eager loading for OfficeAssignment nav prop, 1-to-1/0
                .Include(i => i.Courses) // eager loading for Courses nav prop, 1-to-many
                    .ThenInclude(c => c.Department) // load department for each course, 1-to-1
                .OrderBy(i => i.LastName)
                .ToListAsync();

            // The following code executes when an instructor is selected, that is, id != null.
            if (id != null)
            {
                InstructorID = id.Value;
                // The selected instructor is retrieved from the list of instructors in the view model. 
                Instructor instructor = InstructorData.Instructors
                    .Where(i => i.ID == id.Value).Single(); // The Where method returns a collection. In this case, the filter select a single entity, so the Single method is called to convert the collection into a single Instructor entity.
                // The view model's Courses property is loaded with the Course entities from the selected instructor's Courses navigation property.
                InstructorData.Courses = instructor.Courses; // The Instructor entity provides access to the Course navigation property.
            }

            // The following code populates the view model's Enrollments property when a course is selected:
            if (courseID != null)
            {
                CourseID = courseID.Value;
                var selectedCourse = InstructorData.Courses
                    .Where(x => x.CourseID == courseID).Single(); // The Single() method throws an exception if the collection is empty or if there's more than one item. An alternative is SingleOrDefault()
                await _context.Entry(selectedCourse)
                              .Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
    }
}