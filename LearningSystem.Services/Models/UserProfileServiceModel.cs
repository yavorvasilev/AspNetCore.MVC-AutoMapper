﻿namespace LearningSystem.Services.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserProfileServiceModel : IMapForm<User>, IHaveCustomMapping
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public IEnumerable<UserProfileCourseServiceModel> Courses { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<User, UserProfileServiceModel>()
                .ForMember(u => u.Courses, cfg => cfg.MapFrom(s => s.Courses.Select(c => c.Course)));
    }
}
