using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels;
using VOD.Database.Services;

namespace VOD.Admin.Pages.Users
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IEnumerable<UserDTO> Users = new List<UserDTO>();
        [TempData]
        public string Alert { get; set; }

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task OnGetAsync()
        {
            Users = await _userService.GetUsersAsync();
        }
    }
}
