namespace LearningSystem.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<UserProfileServiceModel> ProfileAsync(string id);

        Task<IEnumerable<UserListingServiceModel>> FindAsync(string searchText);
    }
}
