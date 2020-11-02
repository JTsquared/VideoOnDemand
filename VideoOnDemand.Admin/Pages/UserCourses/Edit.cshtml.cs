using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VideoOnDemand.Admin.Models;
using VideoOnDemand.Admin.Services;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Services;

namespace VideoOnDemand.Admin.Pages.UserCourses
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        private IUserService _userService;

        [BindProperty] public UserCoursePageModel Input { get; set; } = new UserCoursePageModel();
        [TempData] public string StatusMessage { get; set; }

        public EditModel(IDbReadService dbReadService, IDbWriteService dbWriteService, IUserService userService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
            _userService = userService;
        }
        public void OnGet(int courseId, string userId)
        {
            ViewData["Courses"] = _dbReadService.GetSelectList<Course>("Id", "Title");
            Input.UserCourse = _dbReadService.Get<UserCourse>(userId, courseId);
            Input.UpdatedUserCourse = Input.UserCourse;
            var course = _dbReadService.Get<Course>(courseId);
            var user = _userService.GetUser(userId);
            Input.CourseTitle = course.Title;
            Input.Email = user.Email;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var success = await _dbWriteService.Update(Input);

                if (success)
                {
                    var updatedCourse = _dbReadService.Get<Course>(Input.UpdatedUserCourse.CourseId);
                    StatusMessage = $"The [{Input.CourseTitle} | {Input.Email}] combination was changed to [{updatedCourse.Title} | {Input.Email}].";
                    return RedirectToPage("Index");
                }
            }

            ViewData["Courses"] = _dbReadService.GetSelectList<Course>("Id", "Title");

            return Page();
        }
    }
}
