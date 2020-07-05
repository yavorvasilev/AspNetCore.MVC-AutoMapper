namespace LearningSystem.Services
{
    using Models;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<UserProfileServiceModel> ProfileAsync(string id);
    }
}
