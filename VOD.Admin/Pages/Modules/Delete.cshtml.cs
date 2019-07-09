using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Modules
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public ModuleDTO Input { get; set; } =
            new ModuleDTO();
        [TempData]
        public string Alert { get; set; }

        public DeleteModel(IAdminService db )
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id, int courseId)
        {
           
            try
            {
                Input = await _db.SingleAsync<Module, ModuleDTO>(
                    s => s.Id.Equals(id) && s.CourseId.Equals(courseId),true);
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
            var courseId = Input.CourseId;

            if (ModelState.IsValid)
            {
                var succeeded = await _db.DeleteAsync<Module>(d=>
                d.Id.Equals(id) && d.CourseId.Equals(courseId));
                if (succeeded)
                {
                    Alert = $"Deleted Module: {Input.Title}.";
                    return RedirectToPage("Index");
                }
               
            }
            Input = await _db.SingleAsync<Module, ModuleDTO>(s =>
            s.Id.Equals(id) && s.CourseId.Equals(courseId),true);
            return Page();
        }
    }
}
