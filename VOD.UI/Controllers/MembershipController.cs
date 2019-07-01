using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels.UI;
using VOD.Common.Entities;
using VOD.Database.Services;

namespace VOD.UI.Controllers
{
    public class MembershipController : Controller
    {
        private readonly string _userId;
        private readonly IMapper _mapper;
        private readonly IUIReadService _db;

        public MembershipController(
            IHttpContextAccessor httpContextAccessor,
            UserManager<VODUser> userManager,
            IMapper mapper,
            IUIReadService db)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var courseDtoObjects = _mapper.Map<List<CourseDTO>>(
                await _db.GetCourses(_userId));
            return View();
        }

        [HttpGet]
        public IActionResult Course(int id)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Video(int id)
        {
            return View();
        }
    }
}
