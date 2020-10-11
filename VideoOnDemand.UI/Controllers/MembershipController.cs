using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.UI.Models.DTOModels;
using VideoOnDemand.UI.Models.MembershipViewModels;
using VideoOnDemand.UI.Repositories;

namespace VideoOnDemand.UI.Controllers
{
    public class MembershipController : Controller
    {
        private string _userId;
        private IReadRepository _db;
        private readonly IMapper _mapper;

        public MembershipController(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IMapper mapper, IReadRepository db)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _db = db;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var courseDtoObjects = _mapper.Map<List<CourseDTO>>(_db.GetCourses(_userId));
            var dashboardModel = new DashboardViewModel();
            dashboardModel.Courses = new List<List<CourseDTO>>();

            var noOfRows = courseDtoObjects.Count <= 3 ? 1 : courseDtoObjects.Count / 3;

            for (var i = 0; i < noOfRows; i++)
            {
                dashboardModel.Courses.Add(courseDtoObjects.Take(3).ToList());
            }

            return View(dashboardModel);
        }

        [HttpGet]
        public IActionResult Course(int id)
        {
            var course = _db.GetCourse(_userId, id);
            var mappedCourseDTOs = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDTOs = _mapper.Map<InstructorDTO>(course.Instructor);
            var mappedModuleDTOs = _mapper.Map<List<ModuleDTO>>(course.Modules);

            for(var i = 0; i < mappedModuleDTOs.Count; i++)
            {
                mappedModuleDTOs[i].Downloads = 
                    course.Modules[i].Downloads.Count.Equals(0) ? null : _mapper.Map<List<DownloadDTO>>(course.Modules[i].Downloads);

                mappedModuleDTOs[i].Videos =
                    course.Modules[i].Videos.Count.Equals(0) ? null : _mapper.Map<List<VideoDTO>>(course.Modules[i].Videos);
            }

            var courseModel = new CourseViewModel
            {
                Course = mappedCourseDTOs,
                Instructor = mappedInstructorDTOs,
                Modules = mappedModuleDTOs
            };

            return View(courseModel);
        }

        [HttpGet]
        public IActionResult Video(int id)
        {
            var video = _db.GetVideo(_userId, id);
            var course = _db.GetCourse(_userId, video.CourseId);

            var mappedVideoDTO = _mapper.Map<VideoDTO>(video);
            var mappedCourseDTO = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);

            var videos = _db.GetVideos(_userId, video.ModuleId).ToList();
            var count = videos.Count;
            var index = videos.IndexOf(video);
            var previous = videos.ElementAtOrDefault(index - 1);
            var previousId = previous?.Id ?? 0;

            var next = videos.ElementAtOrDefault(index + 1);
            var nextId = next?.Id ?? 0;
            var nextTitle = next?.Title ?? String.Empty;
            var nextThumb = next?.Thumbnail ?? String.Empty;

            var videoModel = new VideoViewModel
            {
                Video = mappedVideoDTO,
                Instructor = mappedInstructorDTO,
                Course = mappedCourseDTO,
                LessonInfo = new LessonInfoDTO
                {
                    LessonNumber = index + 1,
                    NumberOfLessons = count,
                    NextVideoId = nextId,
                    PreviousVideoId = previousId,
                    NextVideoTitle = nextTitle,
                    NextVideoThumbnail = nextThumb,
                    CurrentVideoTitle = video.Title,
                    CurrentVideoThumbnail = video.Thumbnail
                }
            };

            return View(videoModel);
        }
    }
}
