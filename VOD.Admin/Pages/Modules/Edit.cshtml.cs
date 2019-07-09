using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Modules
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public ModuleDTO Input { get; set; } =
            new ModuleDTO();
        [TempData]
        public string Alert { get; set; }

        public EditModel(IAdminService db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id, int courseId)
        {
           
            try
            {
                ViewData["Courses"] =
                   (await _db.GetAsync<Course, CourseDTO>())
                   .ToSelectList("Id", "Title");
                Input = await _db.SingleAsync<Module, ModuleDTO>(s=>s.Id.Equals(id) &&
                s.CourseId.Equals(courseId));
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
                var result = await _db.UpdateAsync<ModuleDTO,Module>(Input);
                if (result)
                {
                    Alert = $"{Input.Title} course was updated.";
                    return RedirectToPage("Index");
                }
               
            }
            ViewData["Courses"] =
                   (await _db.GetAsync<Course, CourseDTO>())
                   .ToSelectList("Id", "Title");
            return Page();
        }
    }
}
