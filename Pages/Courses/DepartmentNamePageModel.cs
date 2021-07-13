using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// this class will replace the DepartmentID (FK) with department names in the drop-down menu in Create and Edit pages
namespace ContosoUniversity.Pages.Courses
{
    // The Create and Edit page model classes will derive from DepartmentNamePageModel, to access helper method
    public class DepartmentNamePageModel : PageModel
    {
        // SelectList contains the list of department names
        public SelectList DepartmentNameSL { get; set; }

        // If method parameter "selectedDepartment" is specified, that department is selected in the SelectList
        public void PopulateDepartmentsDropDownList(SchoolContext _context,
            object selectedDepartment = null)
        {
            var departmentsQuery = from d in _context.Departments
                                   orderby d.Name // Sort by name.
                                   select d;

            DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(),
                        "DepartmentID", "Name", selectedDepartment);
        }
    }
}