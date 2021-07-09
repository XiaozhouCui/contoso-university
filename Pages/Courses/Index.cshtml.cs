using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; }

        // The scaffolding engine specified eager loading for the Department navigation property
        // Eager loading is typically more efficient when the related data needs to be displayed
        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Department) // .Include() specifies eager loading, an alternative is .Select() from LINQ
                .AsNoTracking() // improves performance because the entities returned are not tracked, not tracked because they're not updated in the current context.
                .ToListAsync();
        }
    }
}
