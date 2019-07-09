using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Database.Services;

namespace VOD.Admin.Pages.Users
{
    [Authorize(Roles ="Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IDbReadService _dbReadService;
        private readonly IDbWriteService _dbWriteService;

        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public SelectList AvailableCourses { get; set; }
        [BindProperty, Display(Name ="Available Courses")]
        public int CourseId { get; set; }
        public UserDTO Customer { get; set; }

        public DetailsModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }
        public async Task OnGetAsync(string id)
        {
            await FillViewData(id);
        }

        public async Task<IActionResult> OnPostAddAsync(string userId)
        {
            try
            {
                _dbWriteService.Add(new UserCourse { CourseId = CourseId, UserId = userId });
                var succeeded = await _dbWriteService.SaveChangesAsync();
                
            }
            catch
            {

            }
            await FillViewData(userId);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int courseId, string userId)
        {
            try
            {
                var userCourse = await _dbReadService.SingleAsync<UserCourse>(uc =>
                uc.UserId.Equals(userId) &&
                uc.CourseId.Equals(courseId));

                if (userCourse != null)
                {
                    _dbWriteService.Delete(userCourse);
                    await _dbWriteService.SaveChangesAsync();
                }
            }
            catch
            {

            }
            await FillViewData(userId);
            return Page();
        }
        private async Task FillViewData(string userId)
        {
            var user = await _dbReadService.SingleAsync<VODUser>(u => u.Id.Equals(userId));
            Customer = new UserDTO { Id = user.Id, Email = user.Email };
            _dbReadService.Include<UserCourse>();
            var userCourses = await _dbReadService.GetAsync<UserCourse>(uc => uc.UserId.Equals(userId));
            var userCourseIds = userCourses.Select(c => c.CourseId).ToList();
            Courses = userCourses.Select(c => c.Course).ToList();
            var availableCourses = await _dbReadService.GetAsync<Course>(uc =>
            !userCourseIds.Contains(uc.Id));
            AvailableCourses = availableCourses.ToSelectList("Id", "Title");
        }
    }
}