namespace LearningSystem.Services.Blog.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;

    public class BlogArticleDetailsServiceModel : IMapForm<Article>, IHaveCustomMapping
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
            .CreateMap<Article, BlogArticleDetailsServiceModel>()
            .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName));
    }
}
