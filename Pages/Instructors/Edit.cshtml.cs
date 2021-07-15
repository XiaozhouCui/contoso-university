using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Instructors
{
    // derive from base class
    public class EditModel : InstructorCoursesPageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        // retireve data to populate form
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // eagerly load Instructors with OfficeAssignment and Courses
            Instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Instructor == null)
            {
                return NotFound();
            }

            // use the base class method to populate course assignment data for current instructor, using the AssignedCourseData view model
            PopulateAssignedCourseData(_context, Instructor);
            return Page();
        }

        // Updates the retrieved Instructor entity with values from the model binder.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            // fetch again before submitting, eagerly load Instructors with OfficeAssignment and Courses
            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (instructorToUpdate == null)
            {
                return NotFound();
            }

            // TryUpdateModelAsync prevents overposting, it returns "true" if update is successful
            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate,
                "Instructor",
                i => i.FirstMidName, i => i.LastName,
                i => i.HireDate, i => i.OfficeAssignment))
            {
                // reaching this code block means update is successful
                // handle the one-to-zero-or-one relationship for OfficeAssignment
                if (String.IsNullOrWhiteSpace(
                    instructorToUpdate.OfficeAssignment?.Location))
                {
                    // If the office location is blank, sets Instructor.OfficeAssignment to null.
                    instructorToUpdate.OfficeAssignment = null;
                    // When Instructor.OfficeAssignment is null, the related row in the OfficeAssignment table is deleted.
                }
                // Calls helper method to apply information from the checkboxes to the Instructor entity being edited.
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            // if TryUpdateModelAsync fails. below methods restore the assigned course data entered on the page when it is redisplayed with an error message.
            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(_context, instructorToUpdate);
            return Page();
        }

        // helper method to update instructor with selected courses (checkboxes)
        // Since the Razor page doesn't have a collection of Course entities, the model binder can't automatically update the Courses navigation property. Instead of using the model binder to update the Courses navigation property, that's done in the new UpdateInstructorCourses method. Therefore you need to exclude the Courses property from model binding. This doesn't require any change to the code that calls TryUpdateModelAsync because you're using the overload with declared properties and Courses isn't in the include list.
        public void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            // If no checkboxes were selected, initializes the instructorToUpdate.Courses with an empty collection and returns
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }
            // collections are stored in HashSet objects to improve performance
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.CourseID));

            // checks each course against the ones currently assigned to the instructor versus the ones that were selected in the page.
            foreach (var course in _context.Courses)
            {
                // check if the checkbox for a course is selected
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    // If the checkbox for a course is selected but the course is not in the Instructor.Courses navigation property, the course is added to the collection in the navigation property (Instructor.Courses).
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        // Courses is of type ICollection, which has .Add() method
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    // If the checkbox for a course is not selected, but the course is in the Instructor.Courses navigation property, the course is removed from the navigation property.
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        var courseToRemove = instructorToUpdate.Courses.Single(c => c.CourseID == course.CourseID);
                        instructorToUpdate.Courses.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}
