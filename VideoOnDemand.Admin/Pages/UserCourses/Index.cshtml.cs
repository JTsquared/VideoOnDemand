using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoOnDemand.Admin.Models;
using VideoOnDemand.Admin.Services;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Services;

namespace VideoOnDemand.Admin.Pages.UserCourses
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private IDbReadService _dbReadService;
        public IEnumerable<UserCourse> Items = new List<UserCourse>();
        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }
        public void OnGet()
        {
            Items = _dbReadService.GetWithIncludes<UserCourse>();
        }
    }
}
