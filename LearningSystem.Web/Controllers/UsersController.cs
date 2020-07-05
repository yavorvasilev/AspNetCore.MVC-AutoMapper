namespace LearningSystem.Web.Controllers
{
    using Data.Models;
    using Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private IUserService users;
        private UserManager<User> userManager;

        public UsersController(IUserService users, UserManager<User> userManager)
        {
            this.users = users;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Profile(string username) 
        {
            var user = await userManager.FindByNameAsync(username);

            if (user is null)
            {
                return NotFound();
            }

            var profile = await users.ProfileAsync(user.Id);

            return View(profile);
        }
    }
}
