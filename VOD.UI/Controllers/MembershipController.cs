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
using VOD.UI.Models.MembershipViewModels;

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
            var dashboardModel = new DashboardViewModel();
            dashboardModel.Courses = new List<List<CourseDTO>>();
            var noOfRows = courseDtoObjects.Count <= 3 ? 1 : courseDtoObjects.Count / 3;
            for (int i = 0; i < noOfRows; i++)
            {
                dashboardModel.Courses.Add(
                    courseDtoObjects.Skip(i * 3).Take(3).ToList());
            }
            return View(dashboardModel);
        }

        [HttpGet]
        public async Task<IActionResult> Course(int id)
        {
            var course = await _db.GetCourse(_userId, id);
            var mappedCourseDto = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDto =
                _mapper.Map<InstructorDTO>(course.Instructor);
            var mappedModuleDtos =
                _mapper.Map<List<ModuleDTO>>(course.Modules);
            var courseModel = new CourseViewModel
            {
                Course = mappedCourseDto,
                Instructor = mappedInstructorDto,
                Modules = mappedModuleDtos
            };
            return View(courseModel);
        }
        [HttpGet]
        public async Task<IActionResult> Video(int id)
        {
            var video = await _db.GetVideo(_userId, id);
            var course = await _db.GetCourse(_userId, video.CourseId);

            var videoDto = _mapper.Map<VideoDTO>(video);
            var courseDto = _mapper.Map<CourseDTO>(course);
            var instructorDto = _mapper.Map<InstructorDTO>(course.Instructor);
            var videos = (await _db.GetVideos(_userId, video.ModuleId))
                .OrderBy(o => o.Id).ToList();
            var count = videos.Count();
            var index = videos.FindIndex(v => v.Id.Equals(id));
            var previous = videos.ElementAtOrDefault(index - 1);
            var previousId = previous == null ? 0 : previous.Id;
            var next = videos.ElementAtOrDefault(index + 1);
            var nextId = next == null ? 0 : next.Id;
            var nextTilte = next == null ? string.Empty : next.Title;
            var nextThumb = next == null ? string.Empty : next.Thumbnail;

            var videoModel = new VideoViewModel
            {
                Course = courseDto,
                Instructor = instructorDto,
                Video = videoDto,
                LessonInfo = new LessonInfoDTO
                {
                    LessonNumber = index + 1,
                    NumberOfLessons = count,
                    NextVideoId = nextId,
                    PreviousVideoId = previousId,
                    NextVideoThumbnail = nextThumb,
                    NextVideoTitle = nextTilte,
                    CurrentVideoThumbnail = video.Thumbnail,
                    CurrentVideoTitle = video.Title
                }
            };


            return View(videoModel);
        }
    }
}
