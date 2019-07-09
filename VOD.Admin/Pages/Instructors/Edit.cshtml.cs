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
    public class EditModel : PageModel
    {
        private readonly IAdminService _db;

        [BindProperty]
        public InstructorDTO Input { get; set; } =
            new InstructorDTO();
        [TempData]
        public string Alert { get; set; }

        public EditModel(IAdminService db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                Input = await _db.SingleAsync<Instructor, InstructorDTO>(s=>s.Id.Equals(id));
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
                var result = await _db.UpdateAsync<InstructorDTO,Instructor>(Input);
                if (result)
                {
                    Alert = $"{Input.Name} was updated.";
                    return RedirectToPage("Index");
                }
               
            }
            return Page();
        }
    }
}
