namespace LearningSystem.Services.Blog.Implementations
{
    using LearningSystem.Data;
    using LearningSystem.Data.Models;
    using System;
    using System.Threading.Tasks;

    public class BlogArticleService : IBlogArticleService
    {
        private readonly LearningSystemDbContext db;

        public BlogArticleService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task CreateAsync(
            string title, 
            string content, 
            string authorId)
        {
            var article = new Article
            {
                Title = title,
                Content = content,
                PublishDate = DateTime.UtcNow,
                AuthorId = authorId
            };

            db.Add(article);

            await db.SaveChangesAsync();
        }
    }
}
