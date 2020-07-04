namespace LearningSystem.Web.Controllers
{
    using Data.Models;
    using Models.Courses;
    using Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using LearningSystem.Web.Infrastructure.Extensions;

    public class CoursesController : Controller
    {
        private readonly ICourseService courses;
        private readonly UserManager<User> userManager;

        public CoursesController(
            ICourseService courses, 
            UserManager<User> userManager)
        {
            this.courses = courses;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id) 
        {
            var model = new CourseDetailsViewModel
            {
                Course = await courses.ByIdAsync(id)
            };

            if (model.Course is null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);

                model.UserIsEnrolledCourse = await courses.StudentIsEnrolledCourseAsync(id, userId);
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUp(int id) 
        {
            var userId = userManager.GetUserId(User);

            var success = await courses.SignUpStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Tank you for your registration!");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut(int id) 
        {
            var userId = userManager.GetUserId(User);

            var success = await courses.SignOutStudentAsync(id, userId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("You was unsigned form the course.");

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
