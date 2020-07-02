namespace LearningSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Diagnostics;
    using Services;
    using Microsoft.AspNetCore.Mvc;
    using Web.Models;

    public class HomeController : Controller
    {
        private readonly ICourseService courses;

        public HomeController(ICourseService courses)
        {
            this.courses = courses;
        }

        public async Task<IActionResult> Index()
            => View(await courses.Active());

        public IActionResult Error()
            => View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
    }
}
