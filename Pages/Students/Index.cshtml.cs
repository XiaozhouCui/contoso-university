using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;
        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        // Add properties to contain the sorting parameters
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IList<Student> Students { get; set; }

        // sortOrder comes from query string, it is either Name or Date
        // ?sortOrder=... will be added to query string when the user clicks a column heading link
        // The searchString parameter is received from a text box in Razor Page
        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            // NameSort and DateSort are used by the Razor Page to configure the column heading hyperlinks
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : ""; // using System
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            CurrentFilter = searchString;

            // When an IQueryable object "studentsIQ" is created or modified, no query is sent to the database (deferred)
            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                // LINQ statement Where() clause selects only students whose first name or last name contains the search string.
                // The LINQ statement is executed only if there's a value to search for.

                // case-sensitive filter
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));

                // // case-insensitive filter
                // studentsIQ = studentsIQ.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                //        || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            // modify the IQueryable before execution
            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate); // ascending order by date
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName); // default: ascending order by last name
                    break;
            }
            // the deferred IQueryable studentsIQ is finally executed after .ToListAsync()
            Students = await studentsIQ.AsNoTracking().ToListAsync();
        }
    }
}
