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
    public class DeleteModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        private IUserService _userService;

        [BindProperty] public UserCoursePageModel Input { get; set; } = new UserCoursePageModel();
        [TempData] public string StatusMessage { get; set; }

        public DeleteModel(IDbReadService dbReadService, IDbWriteService dbWriteService, IUserService userService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
            _userService = userService;
        }
        public void OnGet(int courseId, string userId)
        {
            var user = _userService.GetUser(userId);
            var course = _dbReadService.Get<Course>(courseId);
            Input.UserCourse = _dbReadService.Get<UserCourse>(userId, courseId);
            Input.Email = user.Email;
            Input.CourseTitle = course.Title;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var success = await _dbWriteService.Delete(Input.UserCourse);

                if (success)
                {
                    StatusMessage = $"User-Course combination [{Input.CourseTitle} | {Input.Email}] was deleted.";
                    return RedirectToPage("Index");
                }
            }

            return Page();
        }
    }
}
