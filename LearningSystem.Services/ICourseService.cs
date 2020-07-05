namespace LearningSystem.Services
{
    using Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICourseService
    {
        Task<IEnumerable<CourseListingServiceModel>> ActiveAsync();

        Task<TModel> ByIdAsync<TModel>(int id) where TModel : class;

        Task<bool> StudentIsEnrolledCourseAsync(int courseId, string userId);

        Task<bool> SignUpStudentAsync(int courseId, string studentId);

        Task<bool> SignOutStudentAsync(int courseId, string studentId);
    }
}
