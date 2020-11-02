using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Services;

namespace VideoOnDemand.Admin.Pages.Downloads
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;

        [BindProperty] public Download Input { get; set; } = new Download();
        [TempData] public string StatusMessage { get; set; }

        public EditModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }
        public void OnGet(int id)
        {
            ViewData["Modules"] = _dbReadService.GetSelectList<Module>("Id", "Title");
            Input = _dbReadService.Get<Download>(id, true);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Input.CourseId = _dbReadService.Get<Module>(Input.ModuleId).CourseId;
                Input.Course = null;
                var success = await _dbWriteService.Update(Input);

                if (success)
                {
                    StatusMessage = $"Updated Download: {Input.Title}";
                    return RedirectToPage("Index");
                }
            }

            return Page();
        }
    }
}
