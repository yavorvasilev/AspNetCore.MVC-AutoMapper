namespace LearningSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Services;
    using Microsoft.AspNetCore.Mvc;
    using Web.Models;
    using Web.Models.Home;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class HomeController : Controller
    {
        private readonly ICourseService courses;
        private readonly IUserService users;

        public HomeController(ICourseService courses, IUserService users)
        {
            this.courses = courses;
            this.users = users;
        }

        public async Task<IActionResult> Index()
            => View(new HomeIndexViewModel 
            {
                Courses = await courses.ActiveAsync()
            });

        public async Task<IActionResult> Search(SearchFormModel model) 
        {
            var viewModel = new SearchViewModel
            {
                SearchText = model.SearchText
            };

            if (model.SearchInCourses)
            {
                viewModel.Courses = await courses.FindAsync(model.SearchText);
            }

            if (model.SearchInUsers)
            {
                viewModel.Users = await users.FindAsync(model.SearchText);
            }

            return View(viewModel);
        }

        public IActionResult Error()
            => View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
