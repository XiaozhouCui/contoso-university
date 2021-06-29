using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages
{
    public class AboutModel : PageModel
    {
        private readonly SchoolContext _context;

        public AboutModel(SchoolContext context)
        {
            _context = context;
        }

        public IList<EnrollmentDateGroup> Students { get; set; }

        public async Task OnGetAsync()
        {
            // store the results in a collection of EnrollmentDateGroup view model objects
            IQueryable<EnrollmentDateGroup> data =
                // The LINQ statement groups the student entities by enrollment date, called "dateGroup"
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup // e.g. 3 students in group 1/09/2017, 2 students in group 1/09/2018
                // Projects each element of a sequence into a new form 
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key, // e.g. 1/09/2017
                    StudentCount = dateGroup.Count() // e.g. 3
                };

            Students = await data.AsNoTracking().ToListAsync();
        }
    }
}