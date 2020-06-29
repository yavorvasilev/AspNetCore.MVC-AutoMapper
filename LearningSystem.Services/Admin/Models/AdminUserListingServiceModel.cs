namespace LearningSystem.Services.Admin.Models
{
    using Data.Models;
    using Common.Mapping;

    public class AdminUserListingServiceModel : IMapForm<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
