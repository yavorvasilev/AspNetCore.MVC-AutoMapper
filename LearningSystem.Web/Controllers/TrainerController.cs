namespace LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Services;
    using Services.Models;
    using Data.Models;
    using Web.Models.Trainer;

    [Authorize(Roles = WebConstants.TrainerRole)]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainers;
        private readonly ICourseService courses;
        private readonly UserManager<User> userManger;

        public TrainerController(
            ITrainerService trainers, 
            UserManager<User> userManger, 
            ICourseService courses)
        {
            this.trainers = trainers;
            this.userManger = userManger;
            this.courses = courses;
        }

        public async Task<IActionResult> Courses() 
        {
            var userId = userManger.GetUserId(User);
            var courses = await trainers.CoursesAsync(userId);

            return View(courses);
        }

        public async Task<IActionResult> Students(int id) 
        {
            var userId = userManger.GetUserId(User);

            if (!await trainers.IsTrainer(id, userId))
            {
                return NotFound();
            }

            var students = await trainers.StudentsInCourseAsync(id);
            var course = await courses.ByIdAsync<CourseListingServiceModel>(id);

            return View(new StudentsInCourseViewModel
            {
                Students = students,
                Course = course
            });
        }

        [HttpPost]
        public async Task<IActionResult> GradeStudent(int id, string studentId, Grade grade) 
        {
            if (studentId is null)
            {
                return BadRequest();
            }

            var userId = userManger.GetUserId(User);
            if (!await trainers.IsTrainer(id, userId))
            {
                return BadRequest();
            }

            var success = await trainers.AddGrade(id, studentId, grade);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Students), new { id });
        }
    }
}
