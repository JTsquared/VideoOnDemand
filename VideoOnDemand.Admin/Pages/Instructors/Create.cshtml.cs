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
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Services;

namespace VideoOnDemand.Admin.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private IDbWriteService _dbWriteService;

        [BindProperty]
        public Instructor Input { get; set; } = new Instructor();
        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(IDbWriteService dbWriteService)
        {
            _dbWriteService = dbWriteService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var success = await _dbWriteService.Add(Input);

                if (success)
                {
                    StatusMessage = $"Created a new Instructor: {Input.Name}";
                    return RedirectToPage("Index");
                }
            }

            return Page();
        }
    }
}
