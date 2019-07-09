using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Videos
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAdminService _db;
        public IEnumerable<VideoDTO> Items = new List<VideoDTO>();
        [TempData]
        public string Alert { get; set; }

        public IndexModel(IAdminService db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Items = await _db.GetAsync<Video, VideoDTO>(true);
                return Page();
            }
            catch 
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }
    }
}
