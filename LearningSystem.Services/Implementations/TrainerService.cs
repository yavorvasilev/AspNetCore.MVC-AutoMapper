namespace LearningSystem.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LearningSystem.Data.Models;

    public class TrainerService : ITrainerService
    {
        private readonly LearningSystemDbContext db;

        public TrainerService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddGrade(int courseId, string studentId, Grade grade)
        {
            var studentInCourse = await db
                .FindAsync<StudentCourse>(courseId, studentId);

            if (studentInCourse is null)
            {
                return false;
            }

            studentInCourse.Grade = grade;

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> CoursesAsync(string trainerId)
            => await db
            .Courses
            .Where(c => c.TrainerId == trainerId)
            .ProjectTo<CourseListingServiceModel>()
            .ToListAsync();

        public async Task<bool> IsTrainer(int courseId, string trainerId)
            => await db
            .Courses
            .AnyAsync(c => c.TrainerId == trainerId);

        public async Task<IEnumerable<StudentInCourseServiceModel>> StudentsInCourseAsync(int courseId)
            => await db
            .Courses
            .Where(c => c.Id == courseId)
            .SelectMany(c => c.Students.Select(s => s.Student))
            .ProjectTo<StudentInCourseServiceModel>(new { courseId })
            .ToListAsync();
    }
}
