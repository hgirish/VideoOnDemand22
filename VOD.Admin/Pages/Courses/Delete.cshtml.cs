using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public CourseDTO Input { get; set; } =
            new CourseDTO();
        [TempData]
        public string Alert { get; set; }

        public DeleteModel(IAdminService db )
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Input = await _db.SingleAsync<Course, CourseDTO>(s => s.Id.Equals(id),true);
                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." });
            }
          
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var id = Input.Id;

            if (ModelState.IsValid)
            {
                var succeeded = await _db.DeleteAsync<Course>(d=> d.Id.Equals(id));
                if (succeeded)
                {
                    Alert = $"Deleted Course: {Input.Title}.";
                    return RedirectToPage("Index");
                }
               
            }
            Input = await _db.SingleAsync<Course, CourseDTO>(s => s.Id.Equals(id));
            return Page();
        }
    }
}
