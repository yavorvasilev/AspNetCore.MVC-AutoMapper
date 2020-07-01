namespace LearningSystem.Web.Areas.Blog.Models.Articles
{
    using Services.Blog.Models;
    using System;
    using System.Collections.Generic;

    using static Services.ServiceConstants;

    public class ArticleListingViewModel
    {
        public IEnumerable<BlogArticleListingServiceModel> Articles { get; set; }

        public int TotalArticles { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalArticles / BlogArticlesPageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => CurrentPage <= 1 ? 1 : CurrentPage - 1;

        public int NextPage => CurrentPage == TotalPages
            ? TotalPages
            : CurrentPage + 1;
    }
}
