using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WindowsAzure.Storage;
using VideoOnDemand.Admin.Models;
using VideoOnDemand.Admin.Services;

namespace VideoOnDemand.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private IUserService _userService;
        [BindProperty]
        public RegisterUserPageModel Input { get; set; } = new RegisterUserPageModel();
        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddUser(Input);
                if (result.Succeeded)
                {
                    StatusMessage = $"Created a new account for {Input.Email}.";
                    return RedirectToPage("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
