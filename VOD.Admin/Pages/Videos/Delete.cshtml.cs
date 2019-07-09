using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Videos
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public VideoDTO Input { get; set; } =
            new VideoDTO();
        [TempData]
        public string Alert { get; set; }

        public DeleteModel(IAdminService db )
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id,
            int courseId, int moduleId)
        {
            try
            {
                Input = await _db.SingleAsync<Video, VideoDTO>(s =>
                s.Id.Equals(id) &&
                s.ModuleId.Equals(moduleId) &&
                s.CourseId.Equals(courseId),true);
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
            var moduleId = Input.ModuleId;
            var courseId = Input.CourseId;

            if (ModelState.IsValid)
            {
                var succeeded = await _db.DeleteAsync<Video>(s=>
                  s.Id.Equals(id) &&
                s.ModuleId.Equals(moduleId) &&
                s.CourseId.Equals(courseId));

                if (succeeded)
                {
                    Alert = $"Deleted Video: {Input.Title}.";
                    return RedirectToPage("Index");
                }
               
            }
            Input = await _db.SingleAsync<Video, VideoDTO>(s =>
             s.Id.Equals(id) &&
                s.ModuleId.Equals(moduleId) &&
                s.CourseId.Equals(courseId),true);
            return Page();
        }
    }
}
