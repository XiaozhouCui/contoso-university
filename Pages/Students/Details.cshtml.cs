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
    public class DetailsModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public DetailsModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students
                .Include(s => s.Enrollments) // load the Student.Enrollments navigation property
                .ThenInclude(e => e.Course) // within each enrollment, load the Enrollment.Course navigation property
                .AsNoTracking() // improves performance
                .FirstOrDefaultAsync(m => m.ID == id); // better than SingleOrDefaultAsync (throw error if more than one found) and FindAsync (won't retrieve related data)

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
