namespace LearningSystem.Services.Blog.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using LearningSystem.Services.Blog.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static ServiceConstants;

    public class BlogArticleService : IBlogArticleService
    {
        private readonly LearningSystemDbContext db;

        public BlogArticleService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<BlogArticleListingServiceModel>> AllAsync(int page = 1)
            => await db
            .Articles
            .OrderByDescending(a => a.PublishDate)
            .Skip((page - 1) * BlogArticlesPageSize)
            .Take(BlogArticlesPageSize)
            .ProjectTo<BlogArticleListingServiceModel>()
            .ToListAsync();

        public async Task<BlogArticleDetailsServiceModel> ById(int id)
            => await db
            .Articles
            .Where(a => a.Id == id)
            .ProjectTo<BlogArticleDetailsServiceModel>()
            .FirstOrDefaultAsync();

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

        public async Task<int> TotalAsync()
            => await db.Articles.CountAsync();
    }
}
