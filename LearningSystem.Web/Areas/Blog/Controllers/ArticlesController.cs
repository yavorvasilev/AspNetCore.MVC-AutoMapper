namespace LearningSystem.Web.Areas.Blog.Controllers
{
    using Areas.Blog.Models.Articles;
    using Infrastructure.Filters;
    using Data.Models;
    using Services.Blog;
    using Services.Html;
    using Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    using static WebConstants;

    [Area(BlogArea)]
    [Authorize(Roles = BlogAuthorRole)]
    public class ArticlesController : Controller
    {
        private readonly IHtmlService html;
        private readonly IBlogArticleService articles;
        private readonly UserManager<User> userManager;

        public ArticlesController(
            IHtmlService html,
            IBlogArticleService articles,
            UserManager<User> userManager)
        {
            this.html = html;
            this.articles = articles;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
            => View(new ArticleListingViewModel
            {
                Articles = await articles.AllAsync(page),
                TotalArticles = await articles.TotalAsync(),
                CurrentPage = page
            });

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
            => this.ViewOrNotFound(await articles.ById(id));

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create(PublishArticleFormModel model)
        {
            model.Content = html.Sanitize(model.Content);

            var userId = userManager.GetUserId(User);

            await articles.CreateAsync(model.Title, model.Content, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
