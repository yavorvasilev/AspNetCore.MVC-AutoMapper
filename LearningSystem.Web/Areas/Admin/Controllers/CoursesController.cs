namespace LearningSystem.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Areas.Admin.Models.Courses;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using LearningSystem.Web.Controllers;
    using LearningSystem.Services.Admin;
    using LearningSystem.Web.Infrastructure.Extensions;

    public class CoursesController : BaseAdminController
    {
        private readonly UserManager<User> userManager;
        private readonly IAdminCourseService courses;

        public CoursesController(
            UserManager<User> userManager,
            IAdminCourseService courses)
        {
            this.userManager = userManager;
            this.courses = courses;
        }

        public async Task<IActionResult> Create()
            => View(new AddCourseFormModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Trainers = await GetTrainers()
            });

        [HttpPost]
        public async Task<IActionResult> Create(AddCourseFormModel model) 
        {
            if (!ModelState.IsValid)
            {
                model.Trainers = await GetTrainers();
                return View(model);
            }

            await courses.Create(
                model.Name,
                model.Description,
                model.StartDate,
                model.EndDate,
                model.TrainerId);

            TempData.AddSuccessMessage($"Course {model.Name} created successfully.");

            return RedirectToAction(
                nameof(HomeController.Index), 
                "Home", 
                new { area = string.Empty });
        }

        private async Task<IEnumerable<SelectListItem>> GetTrainers() 
        {
            var trainers = await userManager
                .GetUsersInRoleAsync(WebConstants.TrainerRole);

            var trainerListItems = trainers.Select(t => new SelectListItem
            {
                Text = t.UserName,
                Value = t.Id
            })
                .ToList();

            return trainerListItems;
        }
    }
}
