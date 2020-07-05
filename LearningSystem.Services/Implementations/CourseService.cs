namespace LearningSystem.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CourseService : ICourseService
    {
        private readonly LearningSystemDbContext db;

        public CourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> ActiveAsync()
            => await db
            .Courses
            .OrderByDescending(c => c.Id)
            .Where(c => c.StartDate >= DateTime.UtcNow)
            .ProjectTo<CourseListingServiceModel>()
            .ToListAsync();

        public async Task<TModel> ByIdAsync<TModel>(int id) where TModel : class
            => await db
            .Courses
            .Where(c => c.Id == id)
            .ProjectTo<TModel>()
            .FirstOrDefaultAsync();

        public async Task<bool> SignOutStudentAsync(int courseId, string studentId)
        {
            var courseInfo = await GetCourseInfo(courseId, studentId);

            if (courseInfo is null
                || courseInfo.StartDate < DateTime.UtcNow
                || !courseInfo.UserIsEnrolledInCourse)
            {
                return false;
            }

            //var studentInCourseJoins = 
            //    await db
            //    .Courses
            //    .Where(c => c.Id == courseId)
            //    .SelectMany(c => c.Students)
            //    .FirstOrDefaultAsync(s => s.StudentId == studentId);

            var studentInCourse = await db
                .FindAsync<StudentCourse>(courseId, studentId);

            db.Remove(studentInCourse);

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SignUpStudentAsync(int courseId, string studentId)
        {
            var courseInfo = await GetCourseInfo(courseId, studentId);

            if (courseInfo is null 
                || courseInfo.StartDate < DateTime.UtcNow
                || courseInfo.UserIsEnrolledInCourse)
            {
                return false;
            }

            var studentInCourse = new StudentCourse
            {
                CourseId = courseId,
                StudentId = studentId
            };

            db.Add(studentInCourse);

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> StudentIsEnrolledCourseAsync(int courseId, string studentId)
            => await db
            .Courses
            .AnyAsync(c => c.Id == courseId
            && c.Students.Any(s => s.StudentId == studentId));

        private async Task<CourseInfo> GetCourseInfo(int courseId, string studentId)
            => await db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new CourseInfo
                {
                    StartDate = c.StartDate,
                    UserIsEnrolledInCourse = c.Students.Any(s => s.StudentId == studentId)
                })
                .FirstOrDefaultAsync();
    }
}
