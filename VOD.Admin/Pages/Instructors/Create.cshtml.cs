using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;
using VOD.Database.Services;

namespace VOD.Admin.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public InstructorDTO Input { get; set; } =
            new InstructorDTO();
        [TempData]
        public string Alert { get; set; }

        public CreateModel(IAdminService db)
        {
            _db = db;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _db.CreateAsync<InstructorDTO,Instructor>(Input);
                var succeeded = result > 0;
                if (succeeded)
                {
                    Alert = $"Created a new Instructor for {Input.Name}.";
                    return RedirectToPage("Index");
                }
               
            }
            return Page();
        }
    }
}
