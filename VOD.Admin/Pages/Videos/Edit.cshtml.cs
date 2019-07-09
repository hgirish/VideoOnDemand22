using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Videos
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public VideoDTO Input { get; set; } =
            new VideoDTO();
        [TempData]
        public string Alert { get; set; }

        public EditModel(IAdminService db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id,int courseId, int moduleId)
        {
           
            try
            {
                ViewData["Modules"] =
                   (await _db.GetAsync<Module, ModuleDTO>(true))
                   .ToSelectList("Id", "CourseAndModule");
                Input = await _db.SingleAsync<Video, VideoDTO>(s=>
                s.Id.Equals(id) &&
                s.ModuleId.Equals(moduleId) &&
                s.CourseId.Equals(courseId),true);
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
                var moduleId = Input.ModuleId;
                Input.CourseId = (await _db.SingleAsync<Module, ModuleDTO>(
                    s => s.Id.Equals(moduleId))).CourseId;
                var result = await _db.UpdateAsync<VideoDTO,Video>(Input);
                if (result)
                {
                    Alert = $"{Input.Title} course was updated.";
                    return RedirectToPage("Index");
                }
               
            }
            ViewData["Modules"] =
                   (await _db.GetAsync<Module, ModuleDTO>(true))
                   .ToSelectList("Id", "CourseAndModule");
            return Page();
        }
    }
}
