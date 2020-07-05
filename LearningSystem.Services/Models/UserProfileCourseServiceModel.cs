namespace LearningSystem.Services.Models
{
    using Data.Models;
    using Common.Mapping;
    using AutoMapper;
    using System.Linq;

    public class UserProfileCourseServiceModel : IMapForm<Course>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Grade? Grade { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            string studentId = null;
            mapper
                .CreateMap<Course, UserProfileCourseServiceModel>()
                .ForMember(
                p => p.Grade,
                cfg => cfg
                .MapFrom(c => c.Students
                    .Where(s => s.StudentId == studentId)
                    .Select(s => s.Grade)
                    .FirstOrDefault()));
        }
    }
}
