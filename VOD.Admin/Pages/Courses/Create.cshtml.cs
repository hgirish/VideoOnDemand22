using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public CourseDTO Input { get; set; } =
            new CourseDTO();
        [TempData]
        public string Alert { get; set; }

        public CreateModel(IAdminService db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ViewData["Instructors"] =
                    (await _db.GetAsync<Instructor, InstructorDTO>())
                    .ToSelectList("Id","Name");
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new
                {
                    alert = "You do not have access to this page."
                });
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _db.CreateAsync<CourseDTO,Course>(Input);
                var succeeded = result > 0;
                if (succeeded)
                {
                    Alert = $"Created a new Course for {Input.Title}.";
                    return RedirectToPage("Index");
                }
               
            }
            ViewData["Instructors"] =
                    (await _db.GetAsync<Instructor, InstructorDTO>())
                    .ToSelectList("Id", "Name");
            return Page();
        }
    }
}
